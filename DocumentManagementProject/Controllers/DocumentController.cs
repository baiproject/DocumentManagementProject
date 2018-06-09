using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DocumentManagementProject.Controllers
{
    public class DocumentController : Controller
    {
        // GET: Document
        public ActionResult Index()
        {

            var documents = Models.Document.GetAllDocuments();

            var documentList = new List<ViewModels.DocumentViewModel>();           


            foreach (var doc in documents)
            {
                documentList.Add(new ViewModels.DocumentViewModel()
                {
                    Id = doc.Id,
                    Author = doc.Author,
                    Description = doc.Description,
                    FileName = doc.FileName,
                    Title = doc.Title
                });
            }            
            
           
            return View(documentList);
        }



        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Files/"), fileName);
                    file.SaveAs(path);

                    Models.Document.AddDocument(path);
                }
            }

            return RedirectToAction("Index");
        }

       

        public string OpenFile(int id)
        {
            return Models.Document.GetContent(id);            
        }


    }
}