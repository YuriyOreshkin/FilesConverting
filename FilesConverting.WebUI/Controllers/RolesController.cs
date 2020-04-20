using FilesConverting.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FilesConverting.WebUI.Controllers
{
    public class RolesController : ApiController
    {
        public IHttpActionResult Get()
        {
            List<RoleViewModel> roles = new List<RoleViewModel>() {
            new RoleViewModel { ID = 1, NAME = "Администратор", FORALL = true } };
        
            return Ok(roles);
        }
    }
}
