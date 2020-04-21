using FilesConverting.Domain.Repository.Interfaces;
using FilesConverting.WebUI.IoC;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilesConverting.WebUI.Providers
{
    public class CustomRoleProvider : System.Web.Security.RoleProvider
    {
        
        public override string ApplicationName { get { return System.Configuration.ConfigurationManager.AppSettings["ProjectName"]; } set { throw new NotImplementedException(); } }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string login = username.Split('\\')[1];
            var emploeeies = NinjectIoC.Initialize().Get<IEmployeeRepository>();
            var roles = emploeeies.GetEmployeeROLES(this.ApplicationName, login);

            return roles.Select(s => s.ID.ToString()).ToArray();

        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {

           if (GetRolesForUser(username).Contains(roleName))
            
                    return true;
                else
                    return false;
            

        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}