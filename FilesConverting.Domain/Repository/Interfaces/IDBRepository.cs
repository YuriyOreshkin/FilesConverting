using FilesConverting.Domain.Entities;
using System;
using System.Linq;

namespace FilesConverting.Domain.Repository.Interfaces
{
    public interface IDBRepository : IDisposable
    {
        ICRUDRepository<JOURNAL> JOURNAL { get;}
         
    }
}
