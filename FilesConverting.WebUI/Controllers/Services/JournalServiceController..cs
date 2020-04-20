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

                    if (db.JOURNAL.Get().Any(n => n.FILENAME == fileName))
                    {
                        return Content("Файл с таким именем уже существует в базе данных!");
                    }  
                    JOURNAL journal = new JOURNAL();
                    journal.UPLOAD = DateTime.Now;
                    journal.FILENAME = fileName;
                    journal.FILEMIMETYPE = file.ContentType;
                    journal.FILECONTENT = new byte[file.ContentLength];
                    journal.FILESIZE = file.ContentLength;
                    file.InputStream.Read(journal.FILECONTENT, 0, file.ContentLength);

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

        /*//Create
        [HttpPost]
        public ActionResult CreateForGrid([DataSourceRequest]DataSourceRequest request, CategoryViewModel category)
        {
            
            if (ModelState.IsValid)
            {
                CATEGORY entity = category.ToEntity(new CATEGORY());
                try
                {
                    db.CATEGORIES.Create(entity);
                    category.id = entity.ID;

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CATEGORY", ex.Message);
                }
            }

            return Json(new[] { category }.ToDataSourceResult(request, ModelState));

        }

        //Update
        [HttpPost]
        public ActionResult UpdateForGrid([DataSourceRequest]DataSourceRequest request, CategoryViewModel category)
        {
            
                CATEGORY entity = db.CATEGORIES.Get().FirstOrDefault(c => c.ID == category.id);

                if (entity == null)
                {
                    ModelState.AddModelError("CATEGORY", String.Format("Категория '{0}' не обнаружена в базе данных!", category.name));
                }
                else
                {
                    //TODO Validate not found
                    entity = category.ToEntity(entity);
                }

            if (ModelState.IsValid)
            {
                try
                {
                    db.CATEGORIES.Update(entity);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CATEGORY", ex.Message);
                }
            }

            return Json(new[] { category }.ToDataSourceResult(request, ModelState));

        }
        */
        //Delete
        [HttpPost]
        public ActionResult DestroyForGrid([DataSourceRequest]DataSourceRequest request, JournalViewModel journal)
        {
           
                JOURNAL entity = db.JOURNAL.Get().FirstOrDefault(j => j.ID == journal.id);
                if (entity != null)
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