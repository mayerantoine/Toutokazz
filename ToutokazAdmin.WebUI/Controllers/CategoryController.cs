using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Toutokaz.Domain.Models;

namespace ToutokazAdmin.WebUI.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    { 
        ICategoryRepository categoryRepository;
        ISectionRepository sectionRepository;
        
        public CategoryController(ICategoryRepository repository)
        {
            categoryRepository = repository;
        }

        //
        // GET: /Section/

        public ActionResult Index()
        {
            IEnumerable<tb_category> result = categoryRepository.GetAll();
           
            return View(result);
        }

        [HttpGet]
        public ActionResult Create()
        {
            IEnumerable<tb_section> section_list;

            using (sectionRepository = new SectionRepository())
            {
                section_list = sectionRepository.GetAll();
            }

            ViewBag.section = new SelectList(section_list, "id_section", "section_title");
            return View();
        }

        [HttpPost]
        public ActionResult Create(tb_category model)
        {
            if (ModelState.IsValid)
            {

                try
                {
               
                    categoryRepository.Add(model);
                    categoryRepository.Save();
                    ViewBag.Message = "<div class=\"alert alert-success\">!! La section a ete sauvegardee avec succes </div>";
                    return RedirectToAction("Index", "Category");

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
            IEnumerable<tb_section> section_list;

            using (sectionRepository = new SectionRepository())
            {
                section_list = sectionRepository.GetAll();
            }
            ViewBag.section = new SelectList(section_list, "id_section", "section_title");

            tb_category category = categoryRepository.GetById(id);
            if (category != null)
            {
                return View(category);
            }
            return RedirectToAction("Index", "Category");
     
        }

        [HttpPost]
        public ActionResult Edit(tb_category model)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    categoryRepository.Update(model);
                    categoryRepository.Save();
                    ViewBag.Message = "<div class=\"alert alert-success\">!! La section a ete sauvegardee avec succes </div>";
                    return RedirectToAction("Index", "Category");

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
            tb_category category = categoryRepository.GetById(id);
            if (category != null)
            {
                categoryRepository.Delete(category);
                categoryRepository.Save();
                ViewBag.Message = "<div class=\"alert alert-success\">La section a ete supprimee avec succes </div>";
            }
            return RedirectToAction("Index");
        }

    }
}