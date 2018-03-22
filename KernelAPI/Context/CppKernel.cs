using System;
using System.Runtime.InteropServices;

namespace KernelAPI.Context
{
    public class CppKernel : IDisposable, ICppKernel
    {
#if DEBUG
        const string nativeLib = @"./nativelib/libCppLibLinux";
#else
        const string nativeLib = "libCppLibLinux";
#endif

        private IntPtr _ptr;

        public CppKernel(KernelContext ctx)
        {
            _ptr = createKernel(ctx);
        }

        public void StartValidation()
        {
            startValidation(_ptr);
        }

        public void Dispose()
        {
            closeKernel(_ptr);
        }


        [DllImport(nativeLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr createKernel([MarshalAs(UnmanagedType.LPStruct)] KernelContext ctx);

        [DllImport(nativeLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void startValidation(IntPtr intPtrKernel);

        [DllImport(nativeLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void closeKernel(IntPtr intPtrKernel);
    }



    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate string OPEN_WRITE_DATA(string blobName);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate void WRITE_DATA(string blobId, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] buffer, int size);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate void CLOSE_WRITE_DATA(string blobId);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate string OPEN_READ_DATA(string blobId);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate int READ_DATA(string blobId, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] buffer, int size);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate void CLOSE_READ_DATA(string blobId);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate void LOG_DEBUG(string message);

    // CAUTION: change the naming and order of the properties here may cause C++ side to fails

    [StructLayout(LayoutKind.Sequential)]
    public class KernelContext
    {
        public OPEN_WRITE_DATA OpenWriteData;
        public WRITE_DATA WriteData;
        public CLOSE_WRITE_DATA CloseWriteData;
        public OPEN_READ_DATA OpenReadData;
        public READ_DATA ReadData;
        public CLOSE_READ_DATA CloseReadData;
        public LOG_DEBUG LogDebug;
    }

}


