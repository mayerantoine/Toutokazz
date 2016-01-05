using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Toutokaz.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Return view for generic errors (for ex. Internal Server Error like 500).
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Return view for 404 errors.
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFound()
        {
            return View();
        }

    }
}
