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

                r.Add(new UploadFilesResultModel()
                {
                    Name = hpf.FileName,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType
                });
            }
            // Returns json
            return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");
        }
    }
}