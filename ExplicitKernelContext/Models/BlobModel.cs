using System.IO;

namespace ExplicitKernelContext
{
    public class BlobModel
    {
        public string BlobName { get; set; }
        public Stream Stream { get; set; }
        public int CurrentPosition { get; set; }
    }
}
