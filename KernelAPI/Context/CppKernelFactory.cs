namespace KernelAPI.Context
{
    public class CppKernelFactory : ICppKernelFactory
    {
        public ICppKernel Create(KernelContext context)
        {
            return new CppKernel(context);
        }
    }

}


