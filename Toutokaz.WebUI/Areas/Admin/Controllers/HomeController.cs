using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toutokaz.WebUI.Security;

namespace Toutokaz.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        IAuthProvider authProvider;

        public HomeController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult users()
        {
            var query = authProvider.GetAllUser();
           return View(query.ToList());
        }
    }
}
