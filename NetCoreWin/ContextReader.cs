using System;
using System.IO;
using System.Text;

namespace NetCoreWin
{
    public class ContextReader
    {
        private Stream _inStream;
        private int _pos = 0;
        public const string BLOB_ID = "1";

        void ConsoleLog(string message)
        {
            Console.WriteLine($"C#: {message}");
        }


        public string OpenReadDataDelegate(string blobName)
        {
            ConsoleLog($"Open read data - blobName: {blobName} ");
            return BLOB_ID;
        }

        public void CloseReadDataDelegate(string blobId)
        {
            ConsoleLog($"Close read data -  blobId: {blobId}");
        }

        public int ReadDataDelegate(string blobId, byte[] buffer, int size)
        {
            ConsoleLog($"Read data -  blobId: {blobId}");
            return 1;
        }

    }
}
