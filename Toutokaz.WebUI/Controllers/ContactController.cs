using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toutokaz.Domain.Models;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Toutokaz.WebUI.Mailers;

namespace Toutokaz.WebUI.Controllers
{

    public class ContactController : Controller
    {
        IContactRepository contactRepository;

        private IContactMailer _contactMailer = new ContactMailer();
        public IContactMailer contactMailer
        {
            get { return _contactMailer; }
            set { _contactMailer = value; }
        }


        public ContactController(IContactRepository repository)
        {
            contactRepository = repository;
        }

        [HttpGet]
        public ActionResult contact()
        {
            return View();
        }

        [HttpPost]
        public JsonResult contact(tb_contact model)
        {
            if (model != null)
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        contactRepository.Add(model);
                        contactRepository.Save();


                        contactMailer.ContactToutokazz(model).Send();
                    }
                    catch(Exception exp) {

                        return Json(exp.Message, JsonRequestBehavior.AllowGet);
                    }
                    string msg = "Merci de nous contacter, nous vous repondrons prochainement";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string msg = "Une erreur a ete detecte lors dans les valeurs saisie.";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string msg = "Une erreur a ete detecte lors dans les valeurs saisie.";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
