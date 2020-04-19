using FilesConverting.Domain.Entities;
using FilesConverting.Domain.Repository.Interfaces;
using System;
using System.Linq;

namespace FilesConverting.Domain.Repository.Realizations.EF
{
    public class EFRepository : IDBRepository
    {
        private DBContext db = new DBContext();
        private bool disposed = false;


        public ICRUDRepository<JOURNAL> JOURNAL => new EFCRUDRepository<JOURNAL>(db);
        
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
       
    }
}
