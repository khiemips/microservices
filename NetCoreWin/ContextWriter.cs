using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetCoreWin
{
    public class ContextWriter : IDisposable
    {
        //private Stream _outStream;
        //public const int BLOB_ID = 1;

        public string BlobCounter = "0";

        Dictionary<string, BlobWriter> Blobs = new Dictionary<string, BlobWriter>();//implement correct locking during using

        void ConsoleLog(string message)
        {
            Console.WriteLine($"C#: {message}");
        }

        public string OpenWriteDataDelegate(string blobName)
        {
            ConsoleLog($"Open write data - blobName: {blobName}");

            return string.Empty;
        }

        public void CloseWriteDataDelegate(string blobId)
        {
            ConsoleLog($"Close write data - blobId: {blobId}");
        }

        public void WriteDataDelegate(string blobId, byte[] buffer, int size)
        {
            ConsoleLog($"Write data - blobId: {blobId} - buffer: {string.Join('-', buffer)} - size: {size}");
        }

        public class BlobWriter
        {
            public int BlobId;

            public Stream OutStream;

            //possible to add some timeouts ,... watchdogs if needed in future
        }

        public void Dispose()
        {
            //foreach all blobs and dispose thm
        }
    }
}
