using KernelAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KernelAPI
{
    public class ContextReader : IDisposable
    {
        private IStorageService _storageService;
        private Dictionary<string, BlobModel> _readModels;
        private string _blobContainer;

        public ContextReader(IStorageService storageService, string blobContainer)
        {
            _storageService = storageService;
            _readModels = new Dictionary<string, BlobModel>();
            _blobContainer = blobContainer;
        }

        public string OpenRead(string blobId)
        {
            var readModel = _storageService.OpenReadBlobAsync(_blobContainer, blobId).Result;
            _readModels.Add(blobId, readModel);

            return blobId;
        }

        public int Read(string blobId, byte[] buffer, int size)
        {
            var readCount = 0;
            var model = _readModels[blobId];
            using (var binaryReader = new BinaryReader(model.Stream, Encoding.UTF8, leaveOpen: true))
            {
                model.Stream.Position = model.CurrentPosition;
                readCount = binaryReader.Read(buffer, index: 0, count: size);
            }
            model.CurrentPosition += readCount;
            return readCount;
        }

        public void CloseRead(string blobId)
        {
            var model = _readModels[blobId];
            model.Stream.Close();
        }

        public void Dispose()
        {
            foreach (var item in _readModels)
            {
                item.Value.Stream.Close();
            }
        }
    }
}
