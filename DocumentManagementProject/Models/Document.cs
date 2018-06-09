
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DocumentManagementProject.Models
{
    public class Document
    {
        public static List<DocumentModel> GetAllDocuments()
        {
            using (var db = new DocumentDbContext())
            {
                return db.Documents.ToList();
            }
        }

        public static DocumentModel GetDocument(int id)
        {
            using (var db = new DocumentDbContext())
            {
                return db.Documents.Single(x => x.Id == id);
            }
        }

        public static string GetContent(int id)
        {
            string path;
            string fileName;
            string result;
            using (var db = new DocumentDbContext())
            {
                fileName = db.Documents.Single(x => x.Id == id).FileName;
            }

            path = Path.Combine(HttpContext.Current.Server.MapPath("~/Files/"), fileName);

            if (Path.GetExtension(fileName) == ".html")
            {
                result = getHtmlContent(path);
            }else
            {
                result = getDocumentContent(path);
            }            

            return result;
        }

        public static string getHtmlContent(string path)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument() ;
            doc.Load(path);
            
            return doc.DocumentNode?.SelectSingleNode("//div[@id='content']").InnerText;
        }

        public static string getDocumentContent(string path)
        {
            var extractor = new TikaOnDotNet.TextExtraction.TextExtractor();
            var something = extractor.Extract(path);
            return extractor.Extract(path).Text;
        }

        public static void AddDocument(string path)
        {            
            var extractor = new TikaOnDotNet.TextExtraction.TextExtractor();
            var result = extractor.Extract(path);

            string description = result.Metadata?.FirstOrDefault(x => x.Key.ToLower() == "description").Value; //pobiera wartości z metadanych
            string author = result.Metadata?.FirstOrDefault(x => x.Key.ToLower() == "author").Value;
            string title = result.Metadata?.FirstOrDefault(x => x.Key.ToLower() == "title").Value;

            var document = new Models.DocumentModel() {
                FileName = Path.GetFileName(path),
                Author = (author == null) ? "": author,
                Description = (description == null) ? "" : description,
                Title = (title == null) ? "" : title
            };
            using (var db = new DocumentDbContext())
            {
                db.Documents.Add(document);
                db.SaveChanges();
            }
            Services.LuceneSearch.AddUpdateLuceneIndex(document);
        }

    }
}