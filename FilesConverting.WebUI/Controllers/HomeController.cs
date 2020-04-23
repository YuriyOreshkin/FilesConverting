using FilesConverting.Domain.Repository.Interfaces;
using FilesConverting.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilesConverting.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [CustomAuthorize]
        public ActionResult Index()
        {
        
            return View();
        }

        [Authorize]
        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}