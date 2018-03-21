using System.IO;

namespace KernelAPI
{
    public class BlobModel
    {
        public string BlobName { get; set; }
        public Stream Stream { get; set; }
        public int CurrentPosition { get; set; }
    }
}
