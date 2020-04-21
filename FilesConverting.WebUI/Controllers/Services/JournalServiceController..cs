using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Web;
using FilesConverting.Domain.Repository.Interfaces;
using System.Web.Mvc;
using FilesConverting.WebUI.Models;
using FilesConverting.Domain.Entities;
using System.IO;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Text;

namespace FilesConverting.WebUI.Controllers.Services
{
    public class JournalServiceController : Controller
    {
        private IDBRepository db;
        public JournalServiceController(IDBRepository _db)
        {
            db = _db;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();

            base.Dispose(disposing);
        }

        //Read
        public ActionResult ReadForGrid([DataSourceRequest] DataSourceRequest request)
        {
            var journal = db.JOURNAL.Get().ToDataSourceResult(request, j => new JournalViewModel
            {
                id = j.ID,
                upload= j.UPLOAD,
                filename = j.FILENAME,
                filesize = j.FILESIZE,
                modified = j.MODIFIED
            });

            return Json(journal);
        }

        public ActionResult AsyncSave(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    

                    var fileName = Path.GetFileName(file.FileName);
                    //UNIQ name
                    if (db.JOURNAL.Get().Any(n => n.FILENAME == fileName))
                    {
                        return Content("Файл с таким именем уже существует в базе данных!");
                    }

                    //StreamReader inputStreamReader = new StreamReader(file.InputStream, Encoding.UTF8);
                    //string xml_str = inputStreamReader.ReadToEnd();

                    

                    //Save in database
                    JOURNAL journal = new JOURNAL();
                    journal.UPLOAD = DateTime.Now;
                    journal.FILENAME = fileName;
                    journal.FILEMIMETYPE = file.ContentType;
                    journal.FILECONTENT = new byte[file.ContentLength];
                    journal.FILESIZE = file.ContentLength;
                    file.InputStream.Read(journal.FILECONTENT, 0, file.ContentLength);


                    //Проверка xml на согласованность схеме
                    string result = "";
                    using (var ms = new MemoryStream(journal.FILECONTENT))
                    {
                        XmlSchemaSet shema = new XmlSchemaSet();
                        shema.Add("", Server.MapPath(@"~\App_Data\shema.xsd"));

                        XDocument xmldoc = XDocument.Load(ms);


                        xmldoc.Validate(shema, (o, e) =>
                        {
                            result = "Файл не соответствует указаной схеме!" + "\n" + e.Message;

                        });
                    }

                    if (!String.IsNullOrEmpty(result))
                        return Content(result);

                    try
                    {
                        db.JOURNAL.Create(journal);
                    }
                    catch(Exception ex)
                    {
                        
                        return Content(ex.Message);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public FileContentResult GetContent(long id)
        {
            JOURNAL journal = db.JOURNAL.Get().FirstOrDefault(j => j.ID == id);
            if (journal != null)
            {
                return File(journal.FILECONTENT,journal.FILEMIMETYPE,journal.FILENAME);
            }
            else
            {
                return null;
            }
        }

        public ActionResult Modify(long id)
        {
            var entity = db.JOURNAL.Get().FirstOrDefault(j=>j.ID == id);
             if (entity == null)
                 return Json(new { message = "errors", result = "Файл не найден!" }, JsonRequestBehavior.AllowGet);

             try
             {
                 db.Modify(entity);

                 return Json(new { message = "OK" }, JsonRequestBehavior.AllowGet);

             }
             catch (Exception exc)
             {

                 return Json(new { message = "errors", result = exc.Message }, JsonRequestBehavior.AllowGet);
             }

        }

        //Delete
        [HttpPost]
        public ActionResult DestroyForGrid([DataSourceRequest]DataSourceRequest request, JournalViewModel journal)
        {
           
                JOURNAL entity = db.JOURNAL.Get().FirstOrDefault(j => j.ID == journal.id);
                if (entity == null)
                {
                    ModelState.AddModelError("JOURNAL", String.Format("Файл '{0}' не обнаружена в базе данных!", journal.filename));
                }

            if (ModelState.IsValid)
            {
                try
                {
                    db.JOURNAL.Delete(entity);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("JOURNAL", ex.Message);
                }
            }

            return Json(new[] { journal }.ToDataSourceResult(request, ModelState));

        }
        

    }
}