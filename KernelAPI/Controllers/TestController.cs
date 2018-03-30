using ExplicitKernelContext;
using KernelAPI.Attributes;
using KernelAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace KernelAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/test/[action]")]
    [GeneralExceptionFilter]
    public class TestController : Controller
    {
        private readonly IStorageService _storageService;
        private readonly ICppKernelFactory _cppKernelFactory;
        private readonly IConfiguration _config;

        public TestController(IStorageService storageService, ICppKernelFactory cppKernelFactory, IConfiguration config)
        {
            _storageService = storageService;
            _cppKernelFactory = cppKernelFactory;
            _config = config;
        }

        [HttpGet]
        public IActionResult StartValidation()
        {
            // We'll need to define this later;
            var blobContainer = "username";
            var blobFolder = $"sessions/{Guid.NewGuid()}";

            var writer = new ContextWriter(_storageService, blobContainer, blobFolder);
            var reader = new ContextReader(_storageService, blobContainer);

            var kernelContext = new KernelContext
            {
                OpenWriteData = writer.OpenWrite,
                WriteData = writer.Write,
                CloseWriteData = writer.CloseWrite,

                OpenReadData = reader.OpenRead,
                ReadData = reader.Read,
                CloseReadData = reader.CloseRead,

                LogDebug = Console.WriteLine
            };

            using (var kernel = _cppKernelFactory.Create(kernelContext))
            {
                kernel.StartValidation();
            }

            return Ok(new { blobContainer, blobFolder });
        }

        [HttpPost]
        public IActionResult Verify(string blobId)
        {
            // We'll need to define this later;
            var blobContainer = "username";
            var reader = new ContextReader(_storageService, blobContainer);

            reader.OpenRead(blobId);

            // This is to be configurable
            var chunk = new byte[10240];
            var stream = new MemoryStream();

            while (reader.Read(blobId, chunk, chunk.Length) > 0)
            {
                stream.Write(chunk, 0, chunk.Length);
            }

            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/octet-stream");
        }

        [HttpGet]
        public IActionResult Speed()
        {
            var log = new Dictionary<string, string>();
            var stopwatch = new Stopwatch();

            var blobName = $"blob{DateTime.Now.ToString("yyMMddhhmmss")}.pdf";
            var localFile = "./DfmCheckerMock/blob.pdf";
            log.Add("blobName", blobName);

            stopwatch.Start();
            BlobUpload(blobName, localFile);
            log.Add("BlobUpload", $"{stopwatch.ElapsedMilliseconds.ToString("#,###")}ms");

            stopwatch.Restart();
            BlobDownload(blobName, localFile);
            log.Add("BlobDownload", $"{stopwatch.ElapsedMilliseconds.ToString("#,###")}ms");

            stopwatch.Restart();
            RedisUpload(blobName, localFile);
            log.Add("RedisUpload", $"{stopwatch.ElapsedMilliseconds.ToString("#,###")}ms");

            stopwatch.Restart();
            RedisDownload(blobName, localFile);
            log.Add("RedisDownload", $"{stopwatch.ElapsedMilliseconds.ToString("#,###")}ms");

            return Ok(log);
        }


        #region Temp methods

        private void BlobUpload(string blobName, string localFile)
        {
            var blobClient = CloudStorageAccount.Parse(_config["BlobStorage:ConnectionString"]);
            var container = blobClient
                                .CreateCloudBlobClient()
                                .GetContainerReference("performance");
            container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference(blobName);
            blob.UploadFromFileAsync(localFile).Wait();
        }

        private void BlobDownload(string blobName, string localFile)
        {
            var tempFile = Path.GetTempFileName();

            try
            {
                var blobClient = CloudStorageAccount.Parse(_config["BlobStorage:ConnectionString"]);
                var container = blobClient
                                    .CreateCloudBlobClient()
                                    .GetContainerReference("performance");
                container.CreateIfNotExistsAsync();

                var blob = container.GetBlockBlobReference(blobName);
                blob.DownloadToFileAsync(tempFile, FileMode.OpenOrCreate).Wait();
            }
            finally
            {
                System.IO.File.Delete(tempFile);
            }

        }

        private void RedisUpload(string blobName, string localFile)
        {
            var redisClient = new RedisManagerPool(_config["RedisStorage:ConnectionString"])
                .GetClient();

            redisClient.Set(blobName, System.IO.File.ReadAllBytes(localFile));
        }

        private void RedisDownload(string blobName, string localFile)
        {
            var tempFile = Path.GetTempFileName();

            try
            {
                var redisClient = new RedisManagerPool(_config["RedisStorage:ConnectionString"])
                .GetClient();

                System.IO.File.WriteAllBytes(tempFile, redisClient.Get<byte[]>(blobName));
            }
            finally
            {
                System.IO.File.Delete(tempFile);
            }
        }

        #endregion


    }
}
