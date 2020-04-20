using FilesConverting.Domain.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilesConverting.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository db;
        public HomeController(IEmployeeRepository _db)
        {
            db = _db;
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Employee()
        {
            var Employee = db.GetEmployeeROLES("DVOR1", "007OreshkinYV");
            return Content(Employee.First().NAME);
        }

    }
}