using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;
using System.Threading.Tasks;

namespace ExplicitKernelContext.Blob
{
    public class AzureStorageService : IStorageService
    {
        private CloudStorageAccount _storageAccount;

        public AzureStorageService(string accountName, string keyValue)
        {
            _storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, keyValue), useHttps: true);
        }

        public AzureStorageService(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public async Task<BlobModel> OpenWriteBlobAsync(string blobContainer, string blobName)
        {
            var container = _storageAccount
                .CreateCloudBlobClient()
                .GetContainerReference(blobContainer);

            await container.CreateIfNotExistsAsync();

            var blockBlob = container.GetBlockBlobReference(blobName: blobName);

            var blobWriteModel = new BlobModel
            {
                BlobName = blobName,
                Stream = await blockBlob.OpenWriteAsync()
            };

            return blobWriteModel;
        }

        public async Task<BlobModel> OpenReadBlobAsync(string blobContainer, string blobName)
        {
            var container = _storageAccount
                .CreateCloudBlobClient()
                .GetContainerReference(blobContainer);

            var blockBlob = container.GetBlockBlobReference(blobName: blobName);

            var blobReadModel = new BlobModel
            {
                BlobName = blobName,
                Stream = await blockBlob.OpenReadAsync(),
                CurrentPosition = 0
            };

            return blobReadModel;
        }

        public void CloseWrite(Stream stream, string blobName)
        {
            stream.Close();
        }

        public void CloseRead(Stream stream, string blobName)
        {
            stream.Close();
        }
    }
}
