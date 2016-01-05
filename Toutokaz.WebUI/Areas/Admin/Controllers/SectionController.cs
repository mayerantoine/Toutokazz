using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Toutokaz.Domain.Models;
using Toutokaz.WebUI.Areas.Admin.Models;

namespace Toutokaz.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class SectionController : Controller
    {
        ISectionRepository sectionRepository;

        public SectionController(ISectionRepository repository)
        {
            sectionRepository = repository;
        }
        //
        // GET: /Section/

        public ActionResult Index()
        {
            IEnumerable<tb_section> result = sectionRepository.GetAll();

            return View(result);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(SectionViewModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    tb_section section = new tb_section
                    {
                        section_title = model.section_title,
                        section_description = model.section_description
                    };

                    sectionRepository.Add(section);
                    sectionRepository.Save();
                    @ViewBag.Message = "<div class=\"alert alert-success\">!! La section a ete sauvegardee avec succes </div>";
                    //use tempdata
                    return RedirectToAction("Index", "Section");

                }
                catch (Exception exp)
                {
                    ViewBag.Message = exp.Message;
                }
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            tb_section section = sectionRepository.GetById(id);
            if (section != null)
            {
                return View(section);
            }
            return RedirectToAction("Index", "Section");
        }

        [HttpPost]
        public ActionResult Edit(tb_section model)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    sectionRepository.Update(model);
                    sectionRepository.Save();
                    @ViewBag.Message = "<div class=\"alert alert-success\">!! La section a ete sauvegardee avec succes </div>";
                    return RedirectToAction("Index", "Section");

                }
                catch (Exception exp)
                {
                    ViewBag.Message = exp.Message;
                }
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            tb_section section = sectionRepository.GetById(id);
            if (section != null)
            {
                sectionRepository.Delete(section);
                sectionRepository.Save();
                @ViewBag.Message = "<div class=\"alert alert-success\">La section a ete supprimee avec succes </div>";
            }
            return RedirectToAction("Index");
        }

    }
}
