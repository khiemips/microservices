using System;

namespace KernelAPI.Context
{
    public interface ICppKernel : IDisposable
    {
        void StartValidation();
    }
}