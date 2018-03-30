using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExplicitKernelContext
{
    public class ContextWriter : IDisposable
    {
        private Dictionary<string, BlobModel> _writeModels;
        private IStorageService _storageService;

        private string _blobContainer;
        private string _folderPath;

        public ContextWriter(IStorageService storageService, string blobContainer, string folderPath)
        {
            _storageService = storageService;
            _writeModels = new Dictionary<string, BlobModel>();
            _blobContainer = blobContainer;
            _folderPath = folderPath;
        }

        public string OpenWrite(string blobName)
        {
            var blobId = $"{_folderPath}/{blobName}";
            var blobWriteModel = _storageService.OpenWriteBlobAsync(_blobContainer, blobId).Result;
            _writeModels.Add(blobId, blobWriteModel);
            return blobId;
        }

        public void Write(string blobId, byte[] buffer, int size)
        {
            var writeModel = _writeModels[blobId];
            using (var streamWriter = new BinaryWriter(writeModel.Stream, new UTF8Encoding(false, true), true))
            {
                streamWriter.Write(buffer, 0, size);
            }
        }

        public void CloseWrite(string blobId)
        {
            _storageService.CloseWrite(_writeModels[blobId].Stream, blobId);
        }

        public void Dispose()
        {
            foreach (var item in _writeModels)
            {
                item.Value.Stream.Close();
            }
        }


    }
}
