using Ninject;
using FilesConverting.Domain.Repository.Interfaces;
using FilesConverting.Domain.Repository.Realizations.EF;
using FilesConverting.Domain.Repository.Realizations.API;

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
            ninjectKernel.Bind<IEmployeeRepository>().To<APIEmployeeRepository>().InSingletonScope().WithConstructorArgument("api_path", System.Configuration.ConfigurationManager.AppSettings["api"]);

            return ninjectKernel;
        }
    }
}