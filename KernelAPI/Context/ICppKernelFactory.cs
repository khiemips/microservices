namespace KernelAPI.Context
{
    public interface ICppKernelFactory
    {
        ICppKernel Create(KernelContext context);
    }

}


