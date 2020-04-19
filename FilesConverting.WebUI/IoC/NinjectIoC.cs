using Ninject;
using FilesConverting.Domain.Repository.Interfaces;
using FilesConverting.Domain.Repository.Realizations.EF;

namespace FilesConverting.WebUI.IoC
{
    public static class NinjectIoC
    {
        public static IKernel Initialize()
        {
            IKernel kernel = new StandardKernel();
            AddBindings(kernel);
            return kernel;
        }

        private static IKernel AddBindings(IKernel ninjectKernel)
        {
            ninjectKernel.Bind<IDBRepository>().To<EFRepository>();
           
            return ninjectKernel;
        }
    }
}