using Newtonsoft.Json;
using FilesConverting.Domain.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FilesConverting.Domain.Entities;

namespace FilesConverting.Domain.Repository.Realizations.API
{
    public class APIEmployeeRepository : IEmployeeRepository
    {
        private string api_path;
        public APIEmployeeRepository(string api_path)
        {
            this.api_path = api_path;
        }

        public EMPLOYEE GetEmployee(string login)
        {
            using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                UriBuilder builder = new UriBuilder(api_path + "api/getUser");
                builder.Query = "login=" + login;
                var response = client.GetAsync(builder.Uri).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic obj = JsonConvert.DeserializeObject(responseString);
                var employee = obj.UserDetails;
                if (employee != null)
                {
                    return new EMPLOYEE() { ID = employee.UserInfoID, LOGIN = employee.Login, FIRSTNAME = employee.FirstName, LASTNAME = employee.LastName, MIDDLENAME = employee.MiddleName };
                }

                return null;

            }
        }

        public IEnumerable<ROLE> GetEmployeeROLES(string project, string login)
        {
            using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                UriBuilder builder = new UriBuilder(api_path + "api/getUser");
                builder.Query = "login=" + login + "&Project=" + project;
                var response = client.GetAsync(builder.Uri).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic obj = JsonConvert.DeserializeObject(responseString);
                List<ROLE> result = new List<ROLE>();
                var roles = obj.Roles;
                if (roles != null)
                {
                    foreach (var role in roles)
                    {

                        result.Add(new ROLE { ID = role.ID, NAME = role.NAME });

                    }
                }
                return result.AsQueryable();

            }
        }
    }

      
}
