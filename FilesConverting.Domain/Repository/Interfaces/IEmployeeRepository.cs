using FilesConverting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesConverting.Domain.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        EMPLOYEE GetEmployee(string login);

        IEnumerable<ROLE> GetEmployeeROLES(string project, string login);
    }
}
