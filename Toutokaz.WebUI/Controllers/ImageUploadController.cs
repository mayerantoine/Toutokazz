using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc.Mailer;
using System.Web.Security;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Toutokaz.Domain.Models;
using Toutokaz.WebUI.Mailers;
using Toutokaz.WebUI.Models;
using Toutokaz.WebUI.Security;
using PagedList;
using PagedList.Mvc;
using System.Web.Configuration;
using WebMatrix.WebData;
using Toutokaz.WebUI.Helpers;
using System.Web.Script.Serialization;
using System.IO;


namespace Toutokaz.WebUI.Controllers
{
    public class ImageUploadController : Controller
    {
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult Upload()
        {
            return View();

        }

        public JsonResult UploadHandler()
        {
            return Json("OK");
        }

        public ContentResult resultJson()
        {
            var r = new List<UploadFilesResultModel>();
            r.Add(new UploadFilesResultModel()
            {
                Name = "Test",
                Length = 1024,
                Type = "Image",
                url="test"
                
            });          

            return Content("{\"files\":[{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}]}", "application/json");
        }


        [HttpPost]
        public ActionResult deposer(String Titre, String Description, ICollection<String>  photoname)
        {
            var f = Request.Files;
            Console.WriteLine(Titre);
            return View("Index","ImageUpload");
        }

        [HttpPost]
        public ContentResult UploadFiles()
        {
            var r = new List<UploadFilesResultModel>();

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;

                string savedFileName = Path.Combine(Server.MapPath("~/Photos/large/"), Path.GetFileName(hpf.FileName));
                hpf.SaveAs(savedFileName); // Save the file
                var fileName = Path.GetFileName(hpf.FileName);
                var extension = Path.GetExtension(fileName);
                var guid = Guid.NewGuid().ToString();
                //var directory = "adsphotos";
                var filepathlarge = Path.Combine(Server.MapPath("~/Photos/large/"), guid + extension);
                string renamedFile = guid + extension;

                r.Add(new UploadFilesResultModel()
                {
                    Name = renamedFile,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType
                });
            }
            // Returns json
            //return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");
            return Content("{\"files\":[{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}]}", "application/json");
        }


        [HttpGet]
        public ActionResult Display()
        {
            return View();
        }
    }
}