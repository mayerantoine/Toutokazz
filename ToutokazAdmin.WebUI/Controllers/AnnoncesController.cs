using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Toutokaz.Domain.Models;
using ToutokazAdmin.WebUI.Models;
using ImageResizer;
using ImageResizer.Resizing;

namespace ToutokazAdmin.WebUI.Controllers
{
    [Authorize]
    public class AnnoncesController : Controller
    {
        IAnnoncesRepository annoncesRepository;
        IAnnoncesTypeRepository adtypeRepository;
        ICommuneRepository communeRepository;
        IDeviseRepository deviseRepository;
        ISectionRepository secRepository;
        ICategoryRepository catRepository;
        IItemConditionRepository condRepository;
        IAnnoncesImageRepository imageRepository;
        

        public AnnoncesController(IAnnoncesRepository repository)
        {
            annoncesRepository = repository;

            adtypeRepository = new AnnoncesTypeRepository();
            communeRepository = new CommuneRepository();
            deviseRepository = new DeviseRepository();
            condRepository = new ItemConditionRepository();
            secRepository = new SectionRepository();
            catRepository = new CategoryRepository();
            imageRepository = new AnnoncesImageRepository();
        }
        
        //
        // GET: /Annonces/

        public ActionResult Index()
        {
            var results = from c in annoncesRepository.GetAll()
                          select new { c, photos = c.tb_ad_image };

            List<AnnoncesViewModel> annonces = new List<AnnoncesViewModel>();

            foreach( var item in results) {
                AnnoncesViewModel model = new AnnoncesViewModel();

                model.id_ad = item.c.id_ad;
                model.category = item.c.tb_category.category_title;
                model.username = item.c.tb_account.username;
                model.email = item.c.tb_account.email;
                model.ad_title = item.c.ad_title;
                model.ad_status = item.c.ad_status;
                model.ad_date_created = item.c.ad_date_created;
                model.ad_is_published = item.c.ad_is_published;
                model.ad_date_expired = item.c.ad_date_expired;

                 foreach (var s in item.photos)
                {
                    model.image_path = s.image_path;
                }

                 annonces.Add(model);
            }
           
            
            return View(annonces);
        }

        [HttpGet]
        public ActionResult Create()
        {
          
            ViewBag.ad_type = new SelectList(adtypeRepository.GetAll(), "id_ad_type", "description");
            ViewBag.id_commune = new SelectList(communeRepository.GetAll(), "id_commune", "commune");
            ViewBag.id_devise = new SelectList(deviseRepository.GetAll(), "id_devise", "symbole");
            ViewBag.id_category = this.populateCategoryBySection();
            ViewBag.id_item_condition = new SelectList(condRepository.GetAll(), "id_item_condition", "item_condition");
            return View();
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


        [HttpPost]
        public ActionResult Create(tb_ads annonces, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3)
        {
           
            try
            {
             
                IEnumerable<tb_category> category = catRepository.GetAll();
   
                List<HttpPostedFileBase> imageList = new List<HttpPostedFileBase>();
                
                if (image1 != null)
                {
                    imageList.Add(image1);
                }
                if (image2 != null)
                {
                    imageList.Add(image2);
                }
                if (image3 != null)
                {

                    imageList.Add(image3);
                }

                if (annonces.id_category != null)
                {
                    // get section of selected category
                    int? section = category.Where(c => c.id_category == annonces.id_category).Select(x => x.id_section).FirstOrDefault();
                    MembershipUser user = Membership.GetUser();

                    annonces.id_user = (Guid)user.ProviderUserKey;
                    annonces.id_account = 2;
                    annonces.ad_code = Guid.NewGuid().ToString();
                    annonces.ad_date_created = DateTime.Now;
                    annonces.ad_date_expired = DateTime.Now;
                    annonces.id_departement = 1;
                    annonces.ad_name = "test user";
                    annonces.ad_is_published = 0;
                    annonces.ad_status = 1;
                    annonces.id_section = section;
                    ModelState.Clear();


                    // TODO: Add insert logic here
                    if (ModelState.IsValid)
                    {

                        if (imageList.Count > 0)
                        {
                            annoncesRepository.Add(annonces);
                            annoncesRepository.Save();

                              foreach (var file in imageList) {
                 
                                    if (file.ContentLength > 0)
                                    {
                                            var fileName = Path.GetFileName(file.FileName);
                                            var extension = Path.GetExtension(fileName);
                                            var guid = Guid.NewGuid().ToString();
                                            var directory = "C:"; 
                                            var filepathlarge = Path.Combine(directory+"/Photos/large/", guid + extension);
                                            var filepathmedium = Path.Combine(directory+"/Photos/medium/", guid + extension);
                                            var filepaththumbnail = Path.Combine(directory+"/Photos/thumbnail/", guid + extension);

                                            //var filepathlarge = Path.Combine(Server.MapPath("~/Photos/large/"), guid + extension);
                                            //var filepathmedium = Path.Combine(Server.MapPath("~/Photos/medium/"), guid + extension);
                                            //var filepaththumbnail = Path.Combine(Server.MapPath("~/Photos/thumbnail/"), guid + extension);

                                   
                                        tb_ad_image ads_photo = new tb_ad_image
                                        {
                                            ad_code = annonces.ad_code,
                                            id_ad = annonces.id_ad,
                                            image_filename = guid + extension,
                                            image_path = filepaththumbnail
                                        };


                                        file.SaveAs(filepathlarge);

                                        Instructions medium = new Instructions("width=800&height=600&format=jpg&mode=max");
                                        Instructions thumbnail = new Instructions("width=72&height=72&format=jpg&mode=max");

                                        //Let the image builder add the correct extension based on the output file type (which may differ).
                                        ImageJob imedium = new ImageJob(filepathlarge, filepathmedium, medium, false, true);
                                        ImageJob ithumbnail = new ImageJob(filepathlarge, filepaththumbnail, thumbnail, false, true);

                                        imedium.Build();
                                        ithumbnail.Build();

                                        imageRepository.Add(ads_photo);
                                        imageRepository.Save();                                    
                               
                                    }
                                 
                                  }

                                  return RedirectToAction("Index");
                                }
                                else
                                {

                                    annoncesRepository.Add(annonces);
                                    annoncesRepository.Save();

                                    tb_ad_image ads_photo = new tb_ad_image
                                    {
                                        ad_code = annonces.ad_code,
                                        id_ad = annonces.id_ad,
                                        image_filename = "default-img.jpg",
                                        image_path = "~/Photos/default-img.jpg"
                                    };

                                    imageRepository.Add(ads_photo);
                                    imageRepository.Save();



                                   return RedirectToAction("Index");
                          

                                }
                        

                    }

                }
                ModelState.AddModelError("Error:001", "An Error has been detected,please review submitted data.");

                ViewBag.ad_type = new SelectList(adtypeRepository.GetAll(), "id_ad_type", "description");
                ViewBag.id_commune = new SelectList(communeRepository.GetAll(), "id_commune", "commune");
                ViewBag.id_devise = new SelectList(deviseRepository.GetAll(), "id_devise", "symbole");
                ViewBag.id_category = this.populateCategoryBySection();
                ViewBag.id_item_condition = new SelectList(condRepository.GetAll(), "id_item_condition", "item_condition");
                return View(annonces);
            }
            catch (Exception exp)
            {
                ModelState.AddModelError("An Error has been detected,please review submitted data.", exp.Message);

                ViewBag.ad_type = new SelectList(adtypeRepository.GetAll(), "id_ad_type", "description");
                ViewBag.id_commune = new SelectList(communeRepository.GetAll(), "id_commune", "commune");
                ViewBag.id_devise = new SelectList(deviseRepository.GetAll(), "id_devise", "symbole");
                ViewBag.id_category = this.populateCategoryBySection();
                ViewBag.id_item_condition = new SelectList(condRepository.GetAll(), "id_item_condition", "item_condition");
                return View(annonces);
            }
        }

    }
}
