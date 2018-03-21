using KernelAPI.Attributes;
using KernelAPI.Context;
using KernelAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace KernelAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/test/[action]")]
    [GeneralExceptionFilter]
    public class TestController : Controller
    {
        private IConfiguration _config;
        private ILogger _logger;
        private IStorageService _storageService;

        public TestController(IStorageService storageService, IConfiguration config, ILogger<TestController> logger)
        {
            _storageService = storageService;
            _config = config;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult StartValidation()
        {
            _logger.LogDebug("Hello from c#");

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


            using (var kernel = new CppKernel(kernelContext))
            {
                kernel.StartValidation();
            }

            return Ok(new { blobContainer, HttpContext.Session.Id, blobFolder });
        }

        [HttpPost]
        public IActionResult Verify(string blobId)
        {
            // We'll need to define this later;
            var blobContainer = "username";
            var reader = new ContextReader(_storageService, blobContainer);

            reader.OpenRead(blobId);

            var chunk = new byte[10240];

            var stream = new MemoryStream();

            while (reader.Read(blobId, chunk, chunk.Length) > 0)
            {
                stream.Write(chunk, 0, chunk.Length);
            }

            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/octet-stream");


        }

    }
}