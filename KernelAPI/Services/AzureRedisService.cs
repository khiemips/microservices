using ServiceStack.Redis;
using System.IO;
using System.Threading.Tasks;

namespace KernelAPI.Services
{
    public class AzureRedisService : IStorageService
    {
        private IRedisClient _redis;

        public AzureRedisService(string connectionString)
        {
            _redis = new RedisManagerPool(connectionString).GetClient();
        }     
        
        public Task<BlobModel> OpenReadBlobAsync(string blobContainer, string blobName)
        {
            var blockBlob = _redis.Get<byte[]>(blobName);

            var readModel = new BlobModel
            {
                BlobName = blobName,
                Stream = new MemoryStream(blockBlob),
                CurrentPosition = 0
            };

            return Task.FromResult(readModel);
        }

        public Task<BlobModel> OpenWriteBlobAsync(string blobContainer, string blobName)
        {
            var stream = new MemoryStream();

            _redis.Set<byte[]>(blobName, null);

            var blobWriteModel = new BlobModel
            {
                BlobName = blobName,
                Stream = stream
            };

            return Task.FromResult(blobWriteModel);
        }

        public void CloseRead(Stream stream, string blobName)
        {
            stream.Close();
        }

        public void CloseWrite(Stream stream, string blobId)
        {
            using (stream)
            {
                var bytes = ((MemoryStream)stream).ToArray();
                _redis.Set(blobId, bytes);
            }
        }

    }
}
