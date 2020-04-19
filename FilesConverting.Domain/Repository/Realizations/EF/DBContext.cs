using FilesConverting.Domain.Entities;
using System.Data.Entity;

namespace FilesConverting.Domain.Repository.Realizations.EF
{
    public class DBContext : DbContext
    {
        public DbSet<JOURNAL> JOURNALS { get; set; }

    }
}

