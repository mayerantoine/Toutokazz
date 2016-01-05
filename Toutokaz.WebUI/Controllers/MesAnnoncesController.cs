using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Toutokaz.Domain.Models;
using PagedList;
using PagedList.Mvc;
using WebMatrix.WebData;
using Toutokaz.WebUI.Models;

namespace Toutokaz.WebUI.Controllers
{
    public class MesAnnoncesController : Controller
    {

        IAnnoncesRepository annoncesRepository;
        ISectionRepository secRepository;
        ICategoryRepository catRepository;

        IDeviseRepository deviseRepository;
        IItemConditionRepository condRepository;

        public MesAnnoncesController(IAnnoncesRepository repository)
        {
            annoncesRepository = repository;
            secRepository = new SectionRepository();
            catRepository = new CategoryRepository();
            deviseRepository = new DeviseRepository();
            condRepository = new ItemConditionRepository();
           
        }
        //
        // GET: /MesAnnonces/

        public ActionResult Index(String titlekeyword,int? id_category,int? id_ad_status,int page = 1)
        {
            if (!Request.IsAuthenticated)
            {
                throw new HttpException(404, "Access Restricted to logged in members");
            }
            var currentuser = User.Identity.Name;
           // MembershipUser user = Membership.GetUser();
            var user_id = WebSecurity.GetUserId(currentuser);
            ViewBag.id_category = this.populateCategoryBySection();
            ViewBag.id_ad_status = new SelectList(annoncesRepository.GetAllAdsStatus(), "id_ad_status", "status");

          //  var query =  annoncesRepository.GetAdsByUser((Guid)user.ProviderUserKey);
            var query = annoncesRepository.GetAdsByUserId(user_id);

            if (!String.IsNullOrEmpty(titlekeyword) || id_category != null || id_ad_status != null)
            {
                if (!String.IsNullOrEmpty(titlekeyword))
                {
                    query = query.Where(c => c.ad_title.Contains(titlekeyword));
                }

                if (id_category != null) 
                {
                    query = query.Where(c => c.id_category == id_category);
 
                }

                if (id_ad_status != null)
                {
                    query = query.Where(c => c.ad_status == id_ad_status);
                }
            }

            return View(query.OrderByDescending(x => x.ad_date_created).ToPagedList(page, 10));
        }

        [HttpGet]
        public ActionResult Modifier(int id)
        {
            tb_ads ad = annoncesRepository.GetById(id);
            if (ad == null)
            {
                throw new HttpException(404, "Annonce not found");
            }

            ModifierAdsViewModel model = new ModifierAdsViewModel
            {
                id_ad = ad.id_ad,
                ad_title = ad.ad_title,
                id_section = ad.id_section,
                id_category = ad.id_category,
                ad_description = ad.ad_description,
                ad_price = (double)ad.ad_price,
                id_devise =(int) ad.id_devise
            };



            ViewBag.id_devise = new SelectList(deviseRepository.GetAll(), "id_devise", "description",model.id_devise);
            ViewBag.selected_category = this.populateCategoryBySection();

            return View(model);
         }


        [HttpPost]
        public ActionResult Modifier(ModifierAdsViewModel model)
        {
           if(ModelState.IsValid){
               tb_ads ad = annoncesRepository.GetById(model.id_ad);
               if (ad == null)
               {
                   throw new HttpException(404, "Annonce not found");
               }

               int? section = catRepository.GetAll().Where(c => c.id_category == model.id_category).FirstOrDefault().id_section;

               try
               {
                   ad.ad_title = model.ad_title;
                   ad.ad_description = model.ad_description;
                   ad.ad_price =(double) model.ad_price;
                   ad.id_devise = model.id_devise;
                   ad.id_section = (int)section;
                   ad.id_category = model.id_category;
                   ad.ad_phone = ad.ad_phone;

                   annoncesRepository.Update(ad);
                   annoncesRepository.Save();

                   return RedirectToAction("Index", "MesAnnonces");
               }
               catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
               {

                   Exception raise = dbEx;

                   foreach (var validationErrors in dbEx.EntityValidationErrors)
                   {
                       foreach (var validationError in validationErrors.ValidationErrors)
                       {
                           string message = string.Format("{0},{1}",
                               validationErrors.Entry.Entity.ToString(),
                               validationError.ErrorMessage);

                           raise = new InvalidOperationException(message, raise);
                       }
                   }

                   ModelState.AddModelError("", raise.Message);
                   ViewBag.selected_devise = new SelectList(deviseRepository.GetAll(), "id_devise", "description", (object)model.id_devise);
                   ViewBag.selected_category = this.populateCategoryBySection();
                   return View(model);

               }
               catch(Exception exp) {

                   ModelState.AddModelError("", exp.Message);
                   ViewBag.selected_devise = new SelectList(deviseRepository.GetAll(), "id_devise", "description",(object) model.id_devise);
                   ViewBag.selected_category = this.populateCategoryBySection();
                   return View(model);
               }        

           }

           ModelState.AddModelError("", "model not valid");
           ViewBag.selected_devise = new SelectList(deviseRepository.GetAll(), "id_devise", "description", (object)model.id_devise);
           ViewBag.selected_category = this.populateCategoryBySection();
           return View(model);
        }

        public ActionResult Desactiver(int id)
        {
            tb_ads ad = annoncesRepository.GetById(id);
            if (ad == null)
            {
                throw new HttpException(404, "Annonce not found");
            }

            try
            {
                ad.ad_status = 2; // set to inactive
                annoncesRepository.Update(ad);
                annoncesRepository.Save();

                return RedirectToAction("Index", "MesAnnonces");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

                Exception raise = dbEx;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0},{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);

                        raise = new InvalidOperationException(message, raise);
                    }
                }

                throw raise;

            }
            catch (Exception exp)
            {
                throw exp;
            }  
   
        }

        public ActionResult vendu(int id)
        {
            tb_ads ad = annoncesRepository.GetById(id);
            if (ad == null)
            {
                throw new HttpException(404, "Annonce not found");
            }

            try
            {

                ad.ad_status = 4; // set to vendu
                annoncesRepository.Update(ad);
                annoncesRepository.Save();
                return RedirectToAction("Index", "MesAnnonces");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0},{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);

                        raise = new InvalidOperationException(message, raise);
                    }
                }
                TempData["Error"] = raise.Message;
                return RedirectToAction("Index", "MesAnnonces");
            }
            catch (Exception exp)
            {
                TempData["Error"] = exp.Message;
                return RedirectToAction("Index", "MesAnnonces");
            }
                  

        }

        public ActionResult Publier(int id)
        {
            tb_ads ad = annoncesRepository.GetById(id);
            if (ad == null)
            {
                throw new HttpException(404, "Annonce not found");
            }

            //if pending cannot published
            if (ad.ad_status < 3)
            {
                ad.ad_status = 1; // set to active
                annoncesRepository.Update(ad);
                annoncesRepository.Save();
            }

            return RedirectToAction("Index", "MesAnnonces");

        }

        public ActionResult Supprimer(int id)
        {
            tb_ads ad = annoncesRepository.GetById(id);
            if (ad == null)
            {
                throw new HttpException(404, "Annonce not found");
            }
           
            try
            {

                annoncesRepository.DeleteAdsImages(ad);
                annoncesRepository.Delete(ad);
                annoncesRepository.Save();
                return RedirectToAction("Index", "MesAnnonces");
            }
            catch (DataException exp)
            {
                TempData["Error"] = exp.Message;
                return RedirectToAction("Index", "MesAnnonces");

            }    

        }

          //
        // GET: /MesAnnonces/Details/5

        public ActionResult Details(int id)
        {
            var annonce = annoncesRepository.GetById(id);

            if (annonce == null)
            {
                throw new HttpException(404, "Annonce not found");
            }
            return View(annonce);
        }

        //
        // GET: /MesAnnonces/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MesAnnonces/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MesAnnonces/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MesAnnonces/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MesAnnonces/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MesAnnonces/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private IDictionary<string, IEnumerable<SelectListItem>> populateCategoryBySection()
        {


            IEnumerable<tb_section> section = secRepository.GetAll();
            IEnumerable<tb_category> category = catRepository.GetAll();

            IDictionary<string, IEnumerable<SelectListItem>> categoryBySection = new Dictionary<string, IEnumerable<SelectListItem>>();
            foreach (var sec in section)
            {
                var catList = new List<SelectListItem>();
                foreach (var cat in category.Where(c => c.id_section == sec.id_section))
                {
                    catList.Add(new SelectListItem { Value = cat.id_category.ToString(), Text = cat.category_title });
                }
                categoryBySection.Add(sec.section_title, catList);
            }

            return categoryBySection;

        }
    }
}
