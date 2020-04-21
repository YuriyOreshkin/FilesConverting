using FilesConverting.Domain.Repository.Interfaces;
using FilesConverting.WebUI.IoC;
using FilesConverting.WebUI.Models;
using Ninject;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FilesConverting.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            // dependency injection
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
           
        }

        protected void Application_PostAuthenticateRequest()
        {

            if (Request.IsAuthenticated)
            {

                IIdentity user = HttpContext.Current.User.Identity;

                
                CustomPrincipal customPrincipal = new CustomPrincipal(user, NinjectIoC.Initialize().Get<IEmployeeRepository>());

                HttpContext.Current.User = customPrincipal;

            }
        }

    }
}
