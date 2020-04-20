using FilesConverting.Domain.Entities;
using FilesConverting.Domain.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

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

        public void Modify(JOURNAL journal)
        {
            //Attribute("Получатель")=Attribute("ФИО")
            byte[] responseData = journal.FILECONTENT;
            using (var ms = new MemoryStream(responseData))
            {
                XDocument xRoot = XDocument.Load(ms);

                XElement node = xRoot.Root;

                IEnumerable<XElement> rows = node.Descendants().Where(x => x.Name.LocalName == "Строка");
                foreach (XElement row in rows)
                {
                    if (row.Attribute("ФИО") != null && row.Attribute("ФИО").Value != null)
                    {
                        if (row.Attribute("Получатель") != null && row.Attribute("Получатель").Value != null)
                        {
                            row.Attribute("Получатель").Value = row.Attribute("ФИО").Value;
                        }
                    }

                }
                var msModified = new MemoryStream();
                xRoot.Save(msModified);

                journal.FILECONTENT = msModified.ToArray();
                journal.MODIFIED = true;
                this.JOURNAL.Update(journal);
            }
            

        }

    


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        
    }
}
