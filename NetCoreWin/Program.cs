using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace NetCoreWin
{
    class Program
    {
#if DEBUG
        const string nativeLib = @"/nativelib/libCppLibLinux";
#else
        const string nativeLib = "libCppLibLinux";
#endif

        static void Main(string[] args)
        {
            var contextWriter = new ContextWriter();
            var contextReader = new ContextReader();

            var ctx = new Context
            {
                OpenReadData = contextReader.OpenReadDataDelegate,
                CloseReadData = contextReader.CloseReadDataDelegate,
                ReadData = contextReader.ReadDataDelegate,

                OpenWriteData = contextWriter.OpenWriteDataDelegate,
                CloseWriteData = contextWriter.CloseWriteDataDelegate,
                WriteData = contextWriter.WriteDataDelegate,
                LogDebug = Console.WriteLine
            };

            var intPtr = createKernel(ctx);
            startValidation(intPtr);
            closeKernel(intPtr);
        }



        [DllImport(nativeLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr createKernel([MarshalAs(UnmanagedType.LPStruct)] Context ctx);

        [DllImport(nativeLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void startValidation(IntPtr intPtrKernel);

        [DllImport(nativeLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void closeKernel(IntPtr intPtrKernel);
    }
}
