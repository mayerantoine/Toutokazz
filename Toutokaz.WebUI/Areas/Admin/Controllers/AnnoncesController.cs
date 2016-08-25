using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Toutokaz.Domain.Models;
using ImageResizer;
using ImageResizer.Resizing;
using Toutokaz.WebUI.Areas.Admin.Models;
using Toutokaz.WebUI.Models;
using Toutokaz.WebUI.Security;
using PagedList;
using PagedList.Mvc;

namespace Toutokaz.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]
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
        IAuthProvider authProvider;
        IAccountRepository account;

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
            authProvider = new FormsAuthProvider();
            account = new AccountRepository();
        }

        //
        // GET: /Annonces/

        public ActionResult Index(String titlekeyword,string annonceur, int? id_category, int? id_ad_status, int page = 1)
        {
            if (!Request.IsAuthenticated)
            {
                throw new HttpException(404, "Access Restricted to logged in members");
            }
       //     MembershipUser user = Membership.GetUser();
            ViewBag.id_category = this.populateCategoryBySection();
            ViewBag.id_ad_status = new SelectList(annoncesRepository.GetAllAdsStatus(), "id_ad_status", "status");
        
            var query = annoncesRepository.GetAllAds();
            ViewBag.CountMesAnnonces = query.Count();
            ViewBag.CountMesAnnoncesPendantes = query.Where(c => c.ad_status == 3).Count();
            ViewBag.CountMesAnnoncesDesactives = query.Where(c => c.ad_status == 2).Count();

            if (!String.IsNullOrEmpty(titlekeyword) || id_category != null || id_ad_status != null || !String.IsNullOrEmpty(annonceur))
            {
                if (!String.IsNullOrEmpty(annonceur))
                {
                    query = query.Where(c => c.ad_email.Contains(annonceur) || c.tb_user_profile.username.Contains(annonceur) );
                }

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


            return View(query.ToPagedList(page, 10));
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

                            foreach (var file in imageList)
                            {

                                if (file.ContentLength > 0)
                                {
                                    var fileName = Path.GetFileName(file.FileName);
                                    var extension = Path.GetExtension(fileName);
                                    var guid = Guid.NewGuid().ToString();
                                   // var directory = "~";
                                    var filepathlarge = Path.Combine(Server.MapPath("~/Photos/large/"), guid + extension);
                                   // var filepathmedium = Path.Combine(directory + "/Photos/medium/", guid + extension);
                                    var filepaththumbnail = Path.Combine(Server.MapPath("~/Photos/thumbnail/"), guid + extension);

                                    //var filepathlarge = Path.Combine(Server.MapPath("~/Photos/large/"), guid + extension);
                                    //var filepathmedium = Path.Combine(Server.MapPath("~/Photos/medium/"), guid + extension);
                                    //var filepaththumbnail = Path.Combine(Server.MapPath("~/Photos/thumbnail/"), guid + extension);


                                    tb_ad_image ads_photo = new tb_ad_image
                                    {
                                        ad_code = annonces.ad_code,
                                        id_ad = annonces.id_ad,
                                        image_filename = guid + extension,
                                        image_path = "~/Photos/thumbnail/"+ guid + extension
                                    };


                                    file.SaveAs(filepathlarge);

                                   // Instructions medium = new Instructions("width=800&height=600&format=jpg&mode=max");
                                    Instructions thumbnail = new Instructions("width=100&height=100&format=jpg");

                                    //Let the image builder add the correct extension based on the output file type (which may differ).
                                   // ImageJob imedium = new ImageJob(filepathlarge, filepathmedium, medium, false, true);
                                    ImageJob ithumbnail = new ImageJob(filepathlarge, filepaththumbnail, thumbnail, false, true);

                                   // imedium.Build();
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


        public ActionResult details(int id)
        {
            var annonce = annoncesRepository.GetById(id);

            if (annonce == null)
            {
                throw new HttpException(404, "Annonce not found");
            }
            return View(annonce);

        }

        
        public ActionResult Desactiver(int id)
        {
            tb_ads ad = annoncesRepository.GetById(id);
            if (ad == null)
            {
                throw new HttpException(404, "Annonce not found");
            }

            ad.ad_status = 3; // set to pending
            ad.ad_is_published = 0;
            annoncesRepository.Update(ad);
            annoncesRepository.Save();

            return RedirectToAction("Index", "Annonces", new { area="Admin"});

        }


        public ActionResult Valider(int id)
        {
            tb_ads ad = annoncesRepository.GetById(id);
            if (ad == null)
            {
                throw new HttpException(404, "Annonce not found");
            }

            ad.ad_status = 1; // set to active
            ad.ad_is_published = 1;
            annoncesRepository.Update(ad);
            annoncesRepository.Save();

            return RedirectToAction("Index", "Annonces", new { area = "Admin" });

        }



        [HttpGet]
        public ActionResult EditProfile()
        {
            if (Request.IsAuthenticated)
            {
                string username = User.Identity.Name;
                int UserId = WebSecurity.GetUserId(username);
                //  MembershipUser user =  Membership.GetUser(username);

                tb_account acc = account.GetAccountByUserId(UserId);
                var query = annoncesRepository.GetAllAds();
                ViewBag.CountMesAnnonces = query.Count();
                ViewBag.CountMesAnnoncesPendantes = query.Where(c => c.ad_status == 3).Count();
                ViewBag.CountMesAnnoncesDesactives = query.Where(c => c.ad_status == 2).Count();
                //ViewBag.accountId = acc.id_account;
                ProfileViewModel profile = new ProfileViewModel
                {
                    id_account = acc.id_account,
                    Nom = acc.lastname,
                    Prenom = acc.firstname,
                    telephone = acc.telephone,
                    address = acc.adresse

                };

                return View(profile);
            }

            return RedirectToAction("Index", "Annonces", new { area = "Admin" });

        }


        [HttpPost]
        public ActionResult EditProfile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                tb_account acc = account.GetById(model.id_account);
                acc.firstname = model.Prenom;
                acc.lastname = model.Nom;
                acc.telephone = model.telephone;
                acc.adresse = model.address;
                acc.pseudo = model.alias;
                try
                {
                    account.Update(acc);
                    account.Save();
                }
                catch (Exception exp)
                {
                    throw exp;
                }
                return RedirectToAction("index", "Annonces", new { area = "Admin" });
            }

            return View(model);

        }

    }
}
