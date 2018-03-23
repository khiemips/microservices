using KernelAPI.Attributes;
using KernelAPI.Context;
using KernelAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public TestController(IStorageService storageService, ICppKernelFactory cppKernelFactory)
        {
            _storageService = storageService;
            _cppKernelFactory = cppKernelFactory;
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

    }
}