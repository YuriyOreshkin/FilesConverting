using Newtonsoft.Json;
using FilesConverting.Domain.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace FilesConverting.WebUI.Models
{
    public class CustomPrincipal : IPrincipal
    {
        IIdentity identity;
        private long _id;
        private string _login;
        private string _fio;



        public string Login{ get { return this._login; }}
        public long ID { get { return this._id; } }
        public string FIO { get { return this._fio; } }

        public CustomPrincipal(IIdentity identity, IEmployeeRepository api)
        {
            this.identity = identity;
            _login = identity.Name.Split('\\')[1];
            var user = api.GetEmployee(_login);
            if (user != null)
            {
                _id = user.ID;
                _fio = user.LASTNAME + " " + user.FIRSTNAME + " " + user.MIDDLENAME; 
            }
            else
            {
                _id = 0;
                _fio = "Anonymous";
            }
        }

        public IIdentity Identity
        {
            get { return this.identity; }
        }

        public bool IsInRole(string role)
        {
            return System.Web.Security.Roles.IsUserInRole(identity.Name, role);
        }

    }
}