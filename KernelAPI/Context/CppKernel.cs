using ExplicitKernelContext;
using System;
using System.Runtime.InteropServices;

namespace KernelAPI.Context
{
    public class CppKernel : IDisposable, ICppKernel
    {
        const string nativeLib = @"./Lib/libCppLibLinux";

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
}
