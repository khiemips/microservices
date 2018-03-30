using System.IO;
using System.Threading.Tasks;

namespace ExplicitKernelContext
{
    public interface IStorageService
    {
        Task<BlobModel> OpenWriteBlobAsync(string blobContainer, string blobName);
        Task<BlobModel> OpenReadBlobAsync(string blobContainer, string blobName);
        void CloseWrite(Stream stream, string blobName);
        void CloseRead(Stream stream, string blobName);
    }
}
