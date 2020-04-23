using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilesConverting.WebUI.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
{
    public override void OnAuthorization(AuthorizationContext filterContext)
    {
        
        bool auth = filterContext.HttpContext.User.IsInRole("1");
        if(!auth)
        {
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary { 
                { "controller", "Home" }, { "action", "AccessDenied" } 
            });
        }
    }
}
}