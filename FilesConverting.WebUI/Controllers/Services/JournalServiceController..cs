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
                filename = j.FILENAME
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
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    JOURNAL journal = new JOURNAL();
                    journal.UPLOAD = DateTime.Now;
                    journal.FILENAME = fileName;
                    journal.FILEMIMETYPE = file.ContentType;
                    journal.FILECONTENT = new byte[file.ContentLength];
                    journal.FILESIZE = file.ContentLength;
                    file.InputStream.Read(journal.FILECONTENT, 0, file.ContentLength);

                    db.JOURNAL.Create(journal);
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

        //Delete
        [HttpPost]
        public ActionResult DestroyForGrid([DataSourceRequest]DataSourceRequest request, CategoryViewModel category)
        {
           
                CATEGORY entity = db.CATEGORIES.Get().FirstOrDefault(c => c.ID == category.id);
                if (entity == null)
                {
                    ModelState.AddModelError("CATEGORY", String.Format("Категория '{0}' не обнаружена в базе данных!", category.name));
                }

                //Used  in test 
                if (db.TESTS.Get().Any(p => p.CATEGORYID == category.id))
                {
                    ModelState.AddModelError("CATEGORY","Невозможно удалить данную запись!<br>" + String.Format("К категории '{0}' относятся следующие тесты: <ul><li> {1} </li></ul>", category.name, String.Join("</li><li>", db.TESTS.Get().Where(p => p.CATEGORYID == entity.ID).Select(s => s.NAME))));
                }

            if (ModelState.IsValid)
            {
                try
                {
                    db.CATEGORIES.Delete(entity);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CATEGORY", ex.Message);
                }
            }

            return Json(new[] { category }.ToDataSourceResult(request, ModelState));

        }
        */

    }
}