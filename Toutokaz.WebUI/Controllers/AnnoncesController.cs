using ImageResizer;
using MvcSiteMapProvider.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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

namespace Toutokaz.WebUI.Controllers
{
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
        IAccountRepository accRepository;
        IAuthProvider authProvider;
        IAdsVehiculeRepository adsVehiculeRepository;
        IAdsImmobilierRepository adsImmobilierRepository;
        IAdsChaussureRepository adsChaussureRepository;

        private IAnnonceMailer _annonceMailer = new AnnonceMailer();
        public IAnnonceMailer AnnonceMailer
        {
            get { return _annonceMailer; }
            set { _annonceMailer = value; }
        }

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
            accRepository = new AccountRepository();
            adsVehiculeRepository = new AdsVehiculeRepository();
            adsImmobilierRepository = new AdsImmobilierRepository();
            adsChaussureRepository = new AdsChaussureRepository();
        }
        //
        // GET: /Annonces/

        public ActionResult touteslesannonces(int pageNumber = 1)
        {

            CategoryPageModel page = new CategoryPageModel();
            IEnumerable<tb_section> sectionList = secRepository.GetAll();
            
            page.CategoryName = "Toutes les Annonces";
            page.category_view = 1;
            page.selected_section = null;
            page.category = null;
            ViewBag.Title = "Toutokazz - Toutes les annonces - ";

            page.categoryList = sectionList;
            var premium = annoncesRepository.GetPremiumAds().ToList();
            if (premium == null)
                throw new HttpException(404, "Premium Ads not found");
            if (premium != null)
            {
                page.AdsPremium = premium;
            }

            var result = annoncesRepository.GetAllActiveAds();
            var total = result.Count();
            if (result == null)
            { throw new HttpException(404, "Ads  not found"); }

        
            if (result != null)
            {
                page.AdsList = result.ToPagedList(pageNumber,12);
            }
            ViewBag.count_annonce = total;
         //   ViewBag.ad_city = new SelectList(communeRepository.GetAllCommune().OrderBy(x=>x.commune), "id_commune", "commune");
            ViewBag.ad_city = populateCommunetByDepartement();
            ViewBag.ad_user_type = null;
            ViewBag.id_category = this.populateCategoryBySection();
            ViewBag.typeannonce = this.populateTypeAnnonce();

            return View("allannonce", page);
        }

        [HttpGet]
        public ActionResult deposerannonce()
        {
             DeposerAnnonceModel model = new DeposerAnnonceModel();

            model.modelannonce = new tb_ads();
            model.vehicule = new tb_ad_vehicule();
            ViewBag.ad_type = new SelectList(adtypeRepository.GetAll(), "id_ad_type", "description");
            ViewBag.id_commune = new SelectList(communeRepository.GetAllCommune(), "id_commune", "commune");
            ViewBag.id_devise = new SelectList(deviseRepository.GetAll(), "id_devise", "description");
            ViewBag.id_category = this.populateCategoryBySection();
            ViewBag.id_item_condition = new SelectList(condRepository.GetAll(), "id_item_condition", "item_condition");
            ViewBag.id_departement = new SelectList(communeRepository.GetAllDepartement(), "id_departement", "departement");
            ViewBag.id_transmission = new SelectList(annoncesRepository.GetRefItems("transmission"),"ref_item_id","ref_item");
            ViewBag.id_carburant = new SelectList(annoncesRepository.GetRefItems("carburant"), "ref_item_id", "ref_item");
            ViewBag.id_nb_piece = new SelectList(annoncesRepository.GetRefItems("nbpieces"), "ref_item_id", "ref_item");
            ViewBag.id_nb_chambre = new SelectList(annoncesRepository.GetRefItems("nbchambre"), "ref_item_id", "ref_item");
            return View(model);

        }

        [HttpPost]
        public JsonResult deposerannonce(DeposerAnnonceModel deposer, IEnumerable<UploadFileModel> images)
        {
            MembershipUser user = null;
            String  loginEmail = null;
            int user_id = 0;
            ViewBag.Title = "Toutokazz - Deposer annonce- ";

            if (!Request.IsAuthenticated)
            {
                // si le visiteur est loggedin
                if (!authProvider.Authenticate(deposer.login.Email, deposer.login.Password))
                {
                    ModelState.AddModelError("", "Vous devez vous connecter pour deposer une annonce.Votre email ou mot de passe est invalide.");
                    string errormsg = "Vous devez vous connecter pour deposer une annonce.Votre email ou mot de passe est invalide";
                    ViewBag.ad_type = new SelectList(adtypeRepository.GetAll(), "id_ad_type", "description");
                    ViewBag.id_commune = new SelectList(communeRepository.GetAllCommune(), "id_commune", "commune");
                    ViewBag.id_devise = new SelectList(deviseRepository.GetAll(), "id_devise", "description");
                    ViewBag.id_category = this.populateCategoryBySection();
                    ViewBag.id_item_condition = new SelectList(condRepository.GetAll(), "id_item_condition", "item_condition");
                    ViewBag.id_departement = new SelectList(communeRepository.GetAllDepartement(), "id_departement", "departement");
                    ViewBag.id_transmission = new SelectList(annoncesRepository.GetRefItems("transmission"), "ref_item_id", "ref_item");
                    ViewBag.id_carburant = new SelectList(annoncesRepository.GetRefItems("carburant"), "ref_item_id", "ref_item");
                    ViewBag.id_nb_piece = new SelectList(annoncesRepository.GetRefItems("nbpieces"), "ref_item_id", "ref_item");
                    ViewBag.id_nb_chambre = new SelectList(annoncesRepository.GetRefItems("nbchambre"), "ref_item_id", "ref_item");

                    return Json(errormsg, JsonRequestBehavior.AllowGet);
                }

                //user = Membership.GetUser(deposer.login.Email);
                loginEmail = deposer.login.Email;
                user_id = WebSecurity.GetUserId(deposer.login.Email);
                FormsAuthentication.SetAuthCookie(deposer.login.Email, false);
            }
            else
            {
             //   user = Membership.GetUser(User.Identity.Name);
                loginEmail = User.Identity.Name;
                user_id = WebSecurity.GetUserId(User.Identity.Name);
            }

            //set ViewBag Data
            ViewBag.ad_type = new SelectList(adtypeRepository.GetAll(), "id_ad_type", "description");
            ViewBag.id_commune = new SelectList(communeRepository.GetAllCommune(), "id_commune", "commune");
            ViewBag.id_devise = new SelectList(deviseRepository.GetAll(), "id_devise", "description");
            ViewBag.id_category = this.populateCategoryBySection();
            ViewBag.id_item_condition = new SelectList(condRepository.GetAll(), "id_item_condition", "item_condition");
            ViewBag.id_departement = new SelectList(communeRepository.GetAllDepartement(), "id_departement", "departement");
            ViewBag.id_transmission = new SelectList(annoncesRepository.GetRefItems("transmission"), "ref_item_id", "ref_item");
            ViewBag.id_carburant = new SelectList(annoncesRepository.GetRefItems("carburant"), "ref_item_id", "ref_item");
            ViewBag.id_nb_piece = new SelectList(annoncesRepository.GetRefItems("nbpieces"), "ref_item_id", "ref_item");
            ViewBag.id_nb_chambre = new SelectList(annoncesRepository.GetRefItems("nbchambre"), "ref_item_id", "ref_item");
            tb_ads annonces = deposer.modelannonce;
                     
    
            try
            {

                IEnumerable<tb_category> category = catRepository.GetAll();
               
                // intialize model annonce
                if (deposer.modelannonce.id_category == null)
                {
                    string errormsg = "Vous devez selectioner une categorie";
                    return Json(errormsg, JsonRequestBehavior.AllowGet);
                }

                // get section of selected category
                int? section = category.Where(c => c.id_category == deposer.modelannonce.id_category).Select(x => x.id_section).FirstOrDefault();
                // MembershipUser user = Membership.GetUser(User.Identity.Name);
               // int? account = accRepository.GetUserProfile((Guid)user.ProviderUserKey).id_account;
                int account = accRepository.GetAccountByUserId(user_id).id_account;
               // deposer.modelannonce.id_user = (Guid)user.ProviderUserKey;

                deposer.modelannonce.id_account =(int) account;
                deposer.modelannonce.UserId = user_id;
             // deposer.modelannonce.ad_code = code; generated in db
                deposer.modelannonce.ad_date_created = DateTime.Now;
                deposer.modelannonce.ad_date_expired = DateTime.Now.AddDays(60);
                //deposer.modelannonce.ad_email = User.Identity.Name;

                if (String.IsNullOrEmpty(loginEmail)) 
                { deposer.modelannonce.ad_email = "toutokazz@gmail.com"; 
                }
                else { 
                    deposer.modelannonce.ad_email = loginEmail; 
                }
               

                deposer.modelannonce.ad_name = deposer.modelannonce.ad_email;
                deposer.modelannonce.ad_is_published = 0;
                deposer.modelannonce.ad_status = 1;
                deposer.modelannonce.id_section = section;
                deposer.modelannonce.id_item_condition = 3;
                              

                if (deposer.modelannonce.ad_price == null)
                {
                    deposer.modelannonce.ad_price = 0;
                }

                if (deposer.modelannonce.id_devise == null)
                {
                    deposer.modelannonce.id_devise = 1;
                }


                ModelState.Clear();

                // check Model State
                if(!ModelState.IsValid){
                  
                    ModelState.AddModelError("", "Une erreur a ete detecte lors dans les valeurs saisie.");
                    string errormsg = "Une erreur a ete detecte lors dans les valeurs saisie.";
                    return Json(errormsg, JsonRequestBehavior.AllowGet);
                }


                // if for any reason the images list is nulll
                if ( images == null)
                {

                    annoncesRepository.Add(deposer.modelannonce);
                    annoncesRepository.Save();

                    tb_ad_image ads_photo = new tb_ad_image
                    {
                        ad_code = deposer.modelannonce.ad_code,
                        id_ad = deposer.modelannonce.id_ad,
                        image_filename = "default-img.jpg",
                        image_path = "static.toutokazz.com/Photos/default-img.jpg"
                    };

                    // add transaction
                    imageRepository.Add(ads_photo);
                    imageRepository.Save();

                    /// ad vehicule
                    if (deposer.vehicule != null &&
                            (deposer.modelannonce.id_category == 1 || deposer.modelannonce.id_category == 3 || deposer.modelannonce.id_category == 4))
                    {
                        tb_ad_vehicule ads_vehicule = new tb_ad_vehicule
                        {
                            id_ad = deposer.modelannonce.id_ad,
                            annee = deposer.vehicule.annee,
                            id_carburant = deposer.vehicule.id_carburant,
                            mileage = deposer.vehicule.mileage,
                            id_transmission = deposer.vehicule.id_transmission,
                            marque = deposer.vehicule.marque,
                            modele = deposer.vehicule.modele
                        };

                        adsVehiculeRepository.Add(ads_vehicule);
                        adsVehiculeRepository.Save();
                    }

                    /// ad immobilier
                    if (deposer.immobilier != null &&
                            ((deposer.modelannonce.id_category >= 5 && deposer.modelannonce.id_category <= 9) || deposer.modelannonce.id_category == 63 || deposer.modelannonce.id_category == 64))
                    {
                        tb_ad_immobilier ads_immobilier = new tb_ad_immobilier
                        {
                            id_ad = deposer.immobilier.id_ad,
                            id_nb_pieces = deposer.immobilier.id_nb_pieces,
                            id_nb_chambre = deposer.immobilier.id_nb_chambre,
                            surface = deposer.immobilier.surface,
                            loyer = deposer.immobilier.loyer
                        };

                        adsImmobilierRepository.Add(ads_immobilier);
                        adsImmobilierRepository.Save();
                    }

                    /// ad chassure
                    if (deposer.chaussure != null &&
                            (deposer.modelannonce.id_category == 43))
                    {
                        tb_ad_chaussure ads_chaussure = new tb_ad_chaussure
                        {
                            id_ad = deposer.chaussure.id_ad,
                            taille = deposer.chaussure.taille,
                            marque = deposer.chaussure.marque
      
                        };

                        adsChaussureRepository.Add(ads_chaussure);
                        adsChaussureRepository.Save();
                    }
                    string errormsg = "OK";
                    return Json(errormsg, JsonRequestBehavior.AllowGet);
                }

                // if they have uploaded images
                if(images !=null && images.Count() > 0){

                    //all files are null, no photo uploaded
                    if (isPhotoListNullorEmpty(images))
                    {
                        annoncesRepository.Add(deposer.modelannonce);
                        annoncesRepository.Save();

                        tb_ad_image ads_photo = new tb_ad_image
                        {
                            ad_code = deposer.modelannonce.ad_code,
                            id_ad = deposer.modelannonce.id_ad,
                            image_filename = "default-img.jpg",
                            image_path = "static.toutokazz.com/Photos/default-img.jpg"
                        };

                        // add transaction
                        imageRepository.Add(ads_photo);
                        imageRepository.Save();

                        if (deposer.vehicule != null &&
                            (deposer.modelannonce.id_category == 1 || deposer.modelannonce.id_category == 3 || deposer.modelannonce.id_category == 4))
                        {
                            tb_ad_vehicule ads_vehicule = new tb_ad_vehicule
                            {
                                id_ad = deposer.modelannonce.id_ad,
                                annee = deposer.vehicule.annee,
                                id_carburant = deposer.vehicule.id_carburant,
                                mileage = deposer.vehicule.mileage,
                                id_transmission = deposer.vehicule.id_transmission,
                                marque = deposer.vehicule.marque,
                                modele = deposer.vehicule.modele
                            };

                            adsVehiculeRepository.Add(ads_vehicule);
                            adsVehiculeRepository.Save();
                        }

                        /// ad immobilier
                        if (deposer.immobilier != null &&
                            ((deposer.modelannonce.id_category >= 5 && deposer.modelannonce.id_category <= 9) || deposer.modelannonce.id_category == 63 || deposer.modelannonce.id_category == 64))
                        {
                            tb_ad_immobilier ads_immobilier = new tb_ad_immobilier
                            {
                                id_ad = deposer.modelannonce.id_ad,
                                id_nb_pieces = deposer.immobilier.id_nb_pieces,
                                id_nb_chambre = deposer.immobilier.id_nb_chambre,
                                surface = deposer.immobilier.surface,
                                loyer = deposer.immobilier.loyer
                            };

                            adsImmobilierRepository.Add(ads_immobilier);
                            adsImmobilierRepository.Save();
                        }

                        /// ad chassure
                        if (deposer.chaussure != null &&
                            (deposer.modelannonce.id_category == 43))
                        {
                            tb_ad_chaussure ads_chaussure = new tb_ad_chaussure
                            {
                                id_ad = deposer.modelannonce.id_ad,
                                taille = deposer.chaussure.taille,
                                marque = deposer.chaussure.marque

                            };

                            adsChaussureRepository.Add(ads_chaussure);
                            adsChaussureRepository.Save();
                        }
                        string errormsg = "OK";
                        return Json(errormsg, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //at least one photo uploaded

                        // check image extension and size
                        foreach (var fileModel in images)
                        {
                            if (fileModel != null)
                            {
                                if (fileModel.image != null)
                                {
                                    if (fileModel.image.ContentLength > 2097152)
                                    {
                                        ModelState.AddModelError("photo", "The size of the file should not exceed 2MB");

                                        string errormsg = "The size of the file should not exceed 2MB";
                                        return Json(errormsg, JsonRequestBehavior.AllowGet);
                                    }

                                    if (fileModel.image.ContentLength <= 0)
                                    {
                                        ModelState.AddModelError("photo", "The size of the file should be more than 0KB");

                                        string errormsg = "The size of the file should be more than 0KB";
                                        return Json(errormsg, JsonRequestBehavior.AllowGet);
                                    }

                                    var supportedTypes = new[] { "jpg", "jpeg", "png", "JPG" };

                                    var fileExt = System.IO.Path.GetExtension(fileModel.image.FileName).Substring(1);

                                    if (!supportedTypes.Contains(fileExt))
                                    {
                                        ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");

                                        string errormsg = "Invalid type. Only the following types (jpg, jpeg, png) are supported.";
                                        return Json(errormsg, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }

                        annoncesRepository.Add(deposer.modelannonce);
                        annoncesRepository.Save();

                        foreach (var file in images)
                        {
                            if (file.image != null)
                            {
                               String serverpath = WebConfigurationManager.AppSettings["ServerPath"]+"/large/";
                               String serverthumbnail = WebConfigurationManager.AppSettings["ServerPath"]+"/thumbnail/";
                               var fileName = Path.GetFileName(file.image.FileName); 
                               var extension = Path.GetExtension(fileName);
                               var guid = Guid.NewGuid().ToString();
                             //var directory = "adsphotos";
                           // var filepathlarge = Path.Combine(Server.MapPath("~/Photos/large/"), fileName.ToSeoUrl() +guid + extension);
                            var filepathlarge = Path.Combine(serverpath, fileName.ToSeoUrl() + guid + extension);

                          //   var filepaththumbnail = Path.Combine(Server.MapPath("~/Photos/thumbnail/"), fileName.ToSeoUrl() +guid + extension);
                            var filepaththumbnail = Path.Combine(serverthumbnail, fileName.ToSeoUrl() + guid + extension);

                                //var filepathlarge = Path.Combine(Server.MapPath("~/Photos/large/"), guid + extension);
                                //var filepathmedium = Path.Combine(Server.MapPath("~/Photos/medium/"), guid + extension);
                                //var filepaththumbnail = Path.Combine(Server.MapPath("~/Photos/thumbnail/"), guid + extension);
                               

                                tb_ad_image ads_photo = new tb_ad_image
                                {
                                    ad_code = annonces.ad_code,
                                    id_ad = annonces.id_ad,
                                    image_filename = fileName.ToSeoUrl() + guid + extension,
                                    image_path = "static.toutokazz.com/Photos/thumbnail/" + fileName.ToSeoUrl() + guid + extension
                                };

                                //add transaction???

                                imageRepository.Add(ads_photo);
                                imageRepository.Save();

                                //check path valid ??

                                file.image.SaveAs(filepathlarge);

                                // Instructions medium = new Instructions("width=800&height=600&format=jpg&mode=max");
                                Instructions thumbnail = new Instructions("width=100&height=100&format=jpg");

                                //Let the image builder add the correct extension based on the output file type (which may differ).
                                // ImageJob imedium = new ImageJob(filepathlarge, filepathmedium, medium, false, true);
                                ImageJob ithumbnail = new ImageJob(filepathlarge, filepaththumbnail, thumbnail, false, true);

                                // imedium.Build();
                                ithumbnail.Build();

                             
                                
                            }

                        }


                        if (deposer.vehicule != null &&
                            (deposer.modelannonce.id_category == 1 || deposer.modelannonce.id_category == 3 || deposer.modelannonce.id_category == 4)
                            )
                        {
                            tb_ad_vehicule ads_vehicule = new tb_ad_vehicule

                            {
                                id_ad = deposer.modelannonce.id_ad,
                                annee = deposer.vehicule.annee,
                                mileage = deposer.vehicule.mileage,
                                id_carburant = deposer.vehicule.id_carburant,
                                id_transmission = deposer.vehicule.id_transmission,
                                marque = deposer.vehicule.marque,
                                modele = deposer.vehicule.modele
                            };

                            adsVehiculeRepository.Add(ads_vehicule);
                            adsVehiculeRepository.Save();
                        }

                        /// ad immobilier
                        if (deposer.immobilier != null && 
                            ((deposer.modelannonce.id_category >=5 && deposer.modelannonce.id_category <=9) || deposer.modelannonce.id_category==63 || deposer.modelannonce.id_category== 64)
                                )
                        {
                            tb_ad_immobilier ads_immobilier = new tb_ad_immobilier
                            {
                                id_ad = deposer.modelannonce.id_ad,
                                id_nb_pieces = deposer.immobilier.id_nb_pieces,
                                id_nb_chambre = deposer.immobilier.id_nb_chambre,
                                surface = deposer.immobilier.surface,
                                loyer = deposer.immobilier.loyer
                            };

                            adsImmobilierRepository.Add(ads_immobilier);
                            adsImmobilierRepository.Save();
                        }

                        /// ad chassure
                        if (deposer.chaussure != null &&
                            (deposer.modelannonce.id_category == 43)
                            )
                        {
                            tb_ad_chaussure ads_chaussure = new tb_ad_chaussure
                            {
                                id_ad = deposer.modelannonce.id_ad,
                                taille = deposer.chaussure.taille,
                                marque = deposer.chaussure.marque

                            };

                            adsChaussureRepository.Add(ads_chaussure);
                            adsChaussureRepository.Save();
                        }

                        //send email to toutokazz team
                      //  _annonceMailer.deposerannonce(User.Identity.Name, deposer.modelannonce.ad_title, deposer.modelannonce.ad_description).Send();

                        string errormsg1 = "OK";
                        return Json(errormsg1, JsonRequestBehavior.AllowGet);
                    }

                }  //end if  imagecount

                string errormsg2 = "Une erreur a été détectée  dans les valeurs saisies.";
                return Json(errormsg2, JsonRequestBehavior.AllowGet);
            }
            catch(System.Data.Entity.Validation.DbEntityValidationException dbEx) {
                
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
                string errormsgval = "Une erreur a été détectée lors de la validation des données saisies.";
                return Json(raise.Message + raise.StackTrace, JsonRequestBehavior.AllowGet);
            
            }
            catch (Exception exp)
            {
         
                ModelState.AddModelError("", exp.Message);
                string errormsg3 = "Une erreur a été détectée  dans les valeurs saisies.";
                return Json(errormsg3+";"+exp.InnerException.Source+";"+exp.InnerException.StackTrace, JsonRequestBehavior.AllowGet);
            }

           

        }


        [HttpPost]
        public JsonResult ImageUpload(HttpPostedFileBase image)
        {
            if(image == null || image== null){
                return Json("Aucune image n'a ete charge",JsonRequestBehavior.AllowGet);
            }

            if(!ModelState.IsValid){
                return Json("The size of the file should not exceed 2MB or Invalid type. Only the following types (jpg, jpeg, png) are supported."
                    ,JsonRequestBehavior.AllowGet);
            }

            // check image size and extension
            if (image.ContentLength > 2097152)
            {
                ModelState.AddModelError("photo", "The size of the file should not exceed 2MB");

                string errormsg = "The size of the file should not exceed 2MB";
                return Json(errormsg, JsonRequestBehavior.AllowGet);
            }

            if (image.ContentLength <= 0)
            {
                ModelState.AddModelError("photo", "The size of the file should be more than 0KB");

                string errormsg = "The size of the file should be more than 0KB";
                return Json(errormsg, JsonRequestBehavior.AllowGet);
            }

            var supportedTypes = new[] { "jpg", "jpeg", "png", "JPG" };

            var fileExt = System.IO.Path.GetExtension(image.FileName).Substring(1);

            if (!supportedTypes.Contains(fileExt))
            {
                ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");

                string errormsg = "Invalid type. Only the following types (jpg, jpeg, png) are supported.";
                return Json(errormsg, JsonRequestBehavior.AllowGet);
            }

            try
            {
               // String serverpath = WebConfigurationManager.AppSettings["ServerPath"]+"/large/";
               // String serverthumbnail = WebConfigurationManager.AppSettings["ServerPath"]+"/thumbnail/";
                var fileName = Path.GetFileName(image.FileName);
                var extension = Path.GetExtension(fileName);
                var guid = Guid.NewGuid().ToString();
                //var directory = "adsphotos";
               var filepathlarge = Path.Combine(Server.MapPath("~/Photos/large/"), guid + extension);
                string renamedFile = guid + extension;
             //  var filepathlarge = Path.Combine(serverpath, guid + extension);
            
                var filepaththumbnail = Path.Combine(Server.MapPath("~/Photos/thumbnail/"), guid + extension);
               // var filepaththumbnail = Path.Combine(serverthumbnail, guid + extension);

                //var filepathlarge = Path.Combine(Server.MapPath("~/Photos/large/"), guid + extension);
                //var filepathmedium = Path.Combine(Server.MapPath("~/Photos/medium/"), guid + extension);
                //var filepaththumbnail = Path.Combine(Server.MapPath("~/Photos/thumbnail/"), guid + extension);


                image.SaveAs(filepathlarge);

                // Instructions medium = new Instructions("width=800&height=600&format=jpg&mode=max");
                Instructions thumbnail = new Instructions("width=100&height=100&format=jpg");

                //Let the image builder add the correct extension based on the output file type (which may differ).
                // ImageJob imedium = new ImageJob(filepathlarge, filepathmedium, medium, false, true);
                ImageJob ithumbnail = new ImageJob(filepathlarge, filepaththumbnail, thumbnail, false, true);

                // imedium.Build();
                ithumbnail.Build();

                ImageInfoModel ImageInfo = new ImageInfoModel();
                ImageInfo.ImageFileName = renamedFile;
                ImageInfo.ImagePath = filepathlarge;

                return Json(ImageInfo, JsonRequestBehavior.AllowGet);
                   

            }
            catch(Exception exp){
                return Json(exp.Message,JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult ajouterannonce(DeposerAnnonceModel deposer, ICollection<String> photoname)
        {
            MembershipUser user = null;
            String loginEmail = null;
            int user_id = 0;
            ViewBag.Title = "Toutokazz - Deposer annonce- ";

            if (!Request.IsAuthenticated)
            {
                // si le visiteur est loggedin
                if (!authProvider.Authenticate(deposer.login.Email, deposer.login.Password))
                {
                    ModelState.AddModelError("", "Vous devez vous connecter pour deposer une annonce.Votre email ou mot de passe est invalide.");
                    string errormsg = "Vous devez vous connecter pour deposer une annonce.Votre email ou mot de passe est invalide";
                    ViewBag.ad_type = new SelectList(adtypeRepository.GetAll(), "id_ad_type", "description");
                    ViewBag.id_commune = new SelectList(communeRepository.GetAllCommune(), "id_commune", "commune");
                    ViewBag.id_devise = new SelectList(deviseRepository.GetAll(), "id_devise", "description");
                    ViewBag.id_category = this.populateCategoryBySection();
                    ViewBag.id_item_condition = new SelectList(condRepository.GetAll(), "id_item_condition", "item_condition");
                    ViewBag.id_departement = new SelectList(communeRepository.GetAllDepartement(), "id_departement", "departement");
                    ViewBag.id_transmission = new SelectList(annoncesRepository.GetRefItems("transmission"), "ref_item_id", "ref_item");
                    ViewBag.id_carburant = new SelectList(annoncesRepository.GetRefItems("carburant"), "ref_item_id", "ref_item");
                    ViewBag.id_nb_piece = new SelectList(annoncesRepository.GetRefItems("nbpieces"), "ref_item_id", "ref_item");
                    ViewBag.id_nb_chambre = new SelectList(annoncesRepository.GetRefItems("nbchambre"), "ref_item_id", "ref_item");

                    return Json(errormsg, JsonRequestBehavior.AllowGet);
                }

                //user = Membership.GetUser(deposer.login.Email);
                loginEmail = deposer.login.Email;
                user_id = WebSecurity.GetUserId(deposer.login.Email);
                FormsAuthentication.SetAuthCookie(deposer.login.Email, false);
            }
            else
            {
                //   user = Membership.GetUser(User.Identity.Name);
                loginEmail = User.Identity.Name;
                user_id = WebSecurity.GetUserId(User.Identity.Name);
            }

            //set ViewBag Data
            ViewBag.ad_type = new SelectList(adtypeRepository.GetAll(), "id_ad_type", "description");
            ViewBag.id_commune = new SelectList(communeRepository.GetAllCommune(), "id_commune", "commune");
            ViewBag.id_devise = new SelectList(deviseRepository.GetAll(), "id_devise", "description");
            ViewBag.id_category = this.populateCategoryBySection();
            ViewBag.id_item_condition = new SelectList(condRepository.GetAll(), "id_item_condition", "item_condition");
            ViewBag.id_departement = new SelectList(communeRepository.GetAllDepartement(), "id_departement", "departement");
            ViewBag.id_transmission = new SelectList(annoncesRepository.GetRefItems("transmission"), "ref_item_id", "ref_item");
            ViewBag.id_carburant = new SelectList(annoncesRepository.GetRefItems("carburant"), "ref_item_id", "ref_item");
            ViewBag.id_nb_piece = new SelectList(annoncesRepository.GetRefItems("nbpieces"), "ref_item_id", "ref_item");
            ViewBag.id_nb_chambre = new SelectList(annoncesRepository.GetRefItems("nbchambre"), "ref_item_id", "ref_item");
            tb_ads annonces = deposer.modelannonce;


            try
            {

                IEnumerable<tb_category> category = catRepository.GetAll();

                // intialize model annonce
                if (deposer.modelannonce.id_category == null)
                {
                    string errormsg = "Vous devez selectioner une categorie";
                    return Json(errormsg, JsonRequestBehavior.AllowGet);
                }

                // get section of selected category
                int? section = category.Where(c => c.id_category == deposer.modelannonce.id_category).Select(x => x.id_section).FirstOrDefault();
                // MembershipUser user = Membership.GetUser(User.Identity.Name);
                // int? account = accRepository.GetUserProfile((Guid)user.ProviderUserKey).id_account;
                int account = accRepository.GetAccountByUserId(user_id).id_account;
                // deposer.modelannonce.id_user = (Guid)user.ProviderUserKey;

                deposer.modelannonce.id_account = (int)account;
                deposer.modelannonce.UserId = user_id;
                // deposer.modelannonce.ad_code = code; generated in db
                deposer.modelannonce.ad_date_created = DateTime.Now;
                deposer.modelannonce.ad_date_expired = DateTime.Now.AddDays(60);
                //deposer.modelannonce.ad_email = User.Identity.Name;

                if (String.IsNullOrEmpty(loginEmail))
                {
                    deposer.modelannonce.ad_email = "toutokazz@gmail.com";
                }
                else
                {
                    deposer.modelannonce.ad_email = loginEmail;
                }


                deposer.modelannonce.ad_name = deposer.modelannonce.ad_email;
                deposer.modelannonce.ad_is_published = 0;
                deposer.modelannonce.ad_status = 1;
                deposer.modelannonce.id_section = section;
                deposer.modelannonce.id_item_condition = 3;


                if (deposer.modelannonce.ad_price == null)
                {
                    deposer.modelannonce.ad_price = 0;
                }

                if (deposer.modelannonce.id_devise == null)
                {
                    deposer.modelannonce.id_devise = 1;
                }


                ModelState.Clear();

                // check Model State
                if (!ModelState.IsValid)
                {

                    ModelState.AddModelError("", "Une erreur a ete detecte lors dans les valeurs saisie.");
                    string errormsg = "Une erreur a ete detecte lors dans les valeurs saisie.";
                    return Json(errormsg, JsonRequestBehavior.AllowGet);
                }


                // if for any reason the images list is nulll
                if (photoname == null || photoname.Count() == 0)
                {

                    annoncesRepository.Add(deposer.modelannonce);
                    annoncesRepository.Save();

                    tb_ad_image ads_photo = new tb_ad_image
                    {
                        ad_code = deposer.modelannonce.ad_code,
                        id_ad = deposer.modelannonce.id_ad,
                        image_filename = "default-img.jpg",
                        image_path = "static.toutokazz.com/Photos/default-img.jpg"
                    };

                    // add transaction
                    imageRepository.Add(ads_photo);
                    imageRepository.Save();

                    /// ad vehicule
                    if (deposer.vehicule != null &&
                            (deposer.modelannonce.id_category == 1 || deposer.modelannonce.id_category == 3 || deposer.modelannonce.id_category == 4))
                    {
                        tb_ad_vehicule ads_vehicule = new tb_ad_vehicule
                        {
                            id_ad = deposer.modelannonce.id_ad,
                            annee = deposer.vehicule.annee,
                            id_carburant = deposer.vehicule.id_carburant,
                            mileage = deposer.vehicule.mileage,
                            id_transmission = deposer.vehicule.id_transmission,
                            marque = deposer.vehicule.marque,
                            modele = deposer.vehicule.modele
                        };

                        adsVehiculeRepository.Add(ads_vehicule);
                        adsVehiculeRepository.Save();
                    }

                    /// ad immobilier
                    if (deposer.immobilier != null &&
                            ((deposer.modelannonce.id_category >= 5 && deposer.modelannonce.id_category <= 9) || deposer.modelannonce.id_category == 63 || deposer.modelannonce.id_category == 64))
                    {
                        tb_ad_immobilier ads_immobilier = new tb_ad_immobilier
                        {
                            id_ad = deposer.immobilier.id_ad,
                            id_nb_pieces = deposer.immobilier.id_nb_pieces,
                            id_nb_chambre = deposer.immobilier.id_nb_chambre,
                            surface = deposer.immobilier.surface,
                            loyer = deposer.immobilier.loyer
                        };

                        adsImmobilierRepository.Add(ads_immobilier);
                        adsImmobilierRepository.Save();
                    }

                    /// ad chassure
                    if (deposer.chaussure != null &&
                            (deposer.modelannonce.id_category == 43))
                    {
                        tb_ad_chaussure ads_chaussure = new tb_ad_chaussure
                        {
                            id_ad = deposer.chaussure.id_ad,
                            taille = deposer.chaussure.taille,
                            marque = deposer.chaussure.marque

                        };

                        adsChaussureRepository.Add(ads_chaussure);
                        adsChaussureRepository.Save();
                    }
                    string errormsg = "OK";
                    return Json(errormsg, JsonRequestBehavior.AllowGet);
                }

                // if they have uploaded images
                if (photoname != null && photoname.Count() > 0)
                {

                    //at least one photo uploaded

                    // check iif file exist in directory

                    // ajouter annonce
                    annoncesRepository.Add(deposer.modelannonce);
                    annoncesRepository.Save();

                    foreach (var photo in photoname)
                    {
                        if (photo != null)
                        {
                            tb_ad_image ads_photo = new tb_ad_image
                            {
                                ad_code = annonces.ad_code,
                                id_ad = annonces.id_ad,
                                image_filename = photo,
                                image_path = "static.toutokazz.com/Photos/thumbnail/" + photo
                            };

                            //add transaction???

                            imageRepository.Add(ads_photo);
                            imageRepository.Save();

                        }
                    }


                    if (deposer.vehicule != null &&
                        (deposer.modelannonce.id_category == 1 || deposer.modelannonce.id_category == 3 || deposer.modelannonce.id_category == 4)
                        )
                    {
                        tb_ad_vehicule ads_vehicule = new tb_ad_vehicule

                        {
                            id_ad = deposer.modelannonce.id_ad,
                            annee = deposer.vehicule.annee,
                            mileage = deposer.vehicule.mileage,
                            id_carburant = deposer.vehicule.id_carburant,
                            id_transmission = deposer.vehicule.id_transmission,
                            marque = deposer.vehicule.marque,
                            modele = deposer.vehicule.modele
                        };

                        adsVehiculeRepository.Add(ads_vehicule);
                        adsVehiculeRepository.Save();
                    }

                    /// ad immobilier
                    if (deposer.immobilier != null &&
                        ((deposer.modelannonce.id_category >= 5 && deposer.modelannonce.id_category <= 9) || deposer.modelannonce.id_category == 63 || deposer.modelannonce.id_category == 64)
                            )
                    {
                        tb_ad_immobilier ads_immobilier = new tb_ad_immobilier
                        {
                            id_ad = deposer.modelannonce.id_ad,
                            id_nb_pieces = deposer.immobilier.id_nb_pieces,
                            id_nb_chambre = deposer.immobilier.id_nb_chambre,
                            surface = deposer.immobilier.surface,
                            loyer = deposer.immobilier.loyer
                        };

                        adsImmobilierRepository.Add(ads_immobilier);
                        adsImmobilierRepository.Save();
                    }

                    /// ad chassure
                    if (deposer.chaussure != null &&
                        (deposer.modelannonce.id_category == 43)
                        )
                    {
                        tb_ad_chaussure ads_chaussure = new tb_ad_chaussure
                        {
                            id_ad = deposer.modelannonce.id_ad,
                            taille = deposer.chaussure.taille,
                            marque = deposer.chaussure.marque

                        };

                        adsChaussureRepository.Add(ads_chaussure);
                        adsChaussureRepository.Save();
                    }

                    //send email to toutokazz team
                    //  _annonceMailer.deposerannonce(User.Identity.Name, deposer.modelannonce.ad_title, deposer.modelannonce.ad_description).Send();

                    string errormsg1 = "OK";
                    return Json(errormsg1, JsonRequestBehavior.AllowGet);

                }  //end if  imagecount

                string errormsg2 = "Une erreur a été détectée  dans les valeurs saisies.";
                return Json(errormsg2, JsonRequestBehavior.AllowGet);
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
                string errormsgval = "Une erreur a été détectée lors de la validation des données saisies.";
                return Json(raise.Message + raise.StackTrace, JsonRequestBehavior.AllowGet);

            }
            catch (Exception exp)
            {

                ModelState.AddModelError("", exp.Message);
                string errormsg3 = "Une erreur a été détectée  dans les valeurs saisies.";
                return Json(errormsg3 + ";" + exp.InnerException.Source + ";" + exp.InnerException.StackTrace, JsonRequestBehavior.AllowGet);
            }

        }

        
        public ContentResult UploadFiles()
        {
            var r = new List<UploadFilesResultModel>();
            String filename = Request.Files["file_data"].FileName;
          
            foreach(string file in Request.Files) {

                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;

                // Valider Image
                if (hpf == null || hpf == null || hpf.ContentLength == 0)
                {
                    r.Add(new UploadFilesResultModel()
                    {
                        Name = hpf.FileName,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType,
                        Error = "Aucune image n'a ete charge"
                    });
                    return Content("{\"files\":[{\"name\":\"" + r[0].Name + "\",\"error\":\"" + r[0].Error + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}]}", "application/json");
                }

                // check image size and extension
                if (hpf.ContentLength > 2097152)
                {

                    string errormsg = "La taille du fichier ne doit pas dépasser 2MB";
                    r.Add(new UploadFilesResultModel()
                    {
                        Name = hpf.FileName,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType,
                        Error = errormsg
                    });

                    return Content("{\"files\":[{\"name\":\"" + r[0].Name + "\",\"error\":\"" + r[0].Error + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}]}", "application/json");
                }

                if (hpf.ContentLength <= 0)
                {

                    string errormsg = "The size of the file should be more than 0KB";
                    r.Add(new UploadFilesResultModel()
                    {
                        Name = hpf.FileName,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType,
                        Error = errormsg
                    });
                    return Content("{\"files\":[{\"name\":\"" + r[0].Name + "\",\"error\":\"" + r[0].Error + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}]}", "application/json");
                }

                var supportedTypes = new[] { "jpg", "jpeg", "png", "JPG" };

                var fileExt = System.IO.Path.GetExtension(hpf.FileName).Substring(1);

                if (!supportedTypes.Contains(fileExt))
                {
                    string errormsg = "Invalid type. Only the following types (jpg, jpeg, png) are supported.";
                    r.Add(new UploadFilesResultModel()
                    {
                        Name = hpf.FileName,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType,
                        Error = errormsg
                    });

                    return Content("{\"files\":[{\"name\":\"" + r[0].Name + "\",\"error\":\"" + r[0].Error + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}]}", "application/json");
                }

                // rebuild and save file
                try
                {

                    // String serverpath = WebConfigurationManager.AppSettings["ServerPath"]+"/large/";
                    // String serverthumbnail = WebConfigurationManager.AppSettings["ServerPath"]+"/thumbnail/";
                    var fileName = Path.GetFileName(hpf.FileName);
                    var extension = Path.GetExtension(fileName);
                    var guid = Guid.NewGuid().ToString();
                    //var directory = "adsphotos";
                    var filepathlarge = Path.Combine(Server.MapPath("~/Photos/large/"), guid + extension);
                    string renamedFile = guid + extension;
                    //  var filepathlarge = Path.Combine(serverpath, guid + extension);

                    var filepaththumbnail = Path.Combine(Server.MapPath("~/Photos/thumbnail/"), guid + extension);
                    // var filepaththumbnail = Path.Combine(serverthumbnail, guid + extension);

                    //var filepathlarge = Path.Combine(Server.MapPath("~/Photos/large/"), guid + extension);
                    //var filepathmedium = Path.Combine(Server.MapPath("~/Photos/medium/"), guid + extension);
                    //var filepaththumbnail = Path.Combine(Server.MapPath("~/Photos/thumbnail/"), guid + extension);


                    hpf.SaveAs(filepathlarge);

                    // Instructions medium = new Instructions("width=800&height=600&format=jpg&mode=max");
                    Instructions thumbnail = new Instructions("width=100&height=100&format=jpg");

                    //Let the image builder add the correct extension based on the output file type (which may differ).
                    // ImageJob imedium = new ImageJob(filepathlarge, filepathmedium, medium, false, true);
                    ImageJob ithumbnail = new ImageJob(filepathlarge, filepaththumbnail, thumbnail, false, true);

                    // imedium.Build();
                    ithumbnail.Build();

                    ImageInfoModel ImageInfo = new ImageInfoModel();
                    ImageInfo.ImageFileName = renamedFile;
                    ImageInfo.ImagePath = filepathlarge;

                    r.Add(new UploadFilesResultModel()
                    {
                        Name = renamedFile,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType

                    });
                }
                catch (Exception exp)
                {

                    r.Add(new UploadFilesResultModel()
                    {
                        Name = hpf.FileName,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType,
                        Error = exp.InnerException.Message

                    });

                    return Content("{\"files\":[{\"name\":\"" + r[0].Name + "\",\"error\":\"" + r[0].Error + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}]}", "application/json");
                }
            }
             // Returns json
            return Content("{\"files\":[{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}]}", "application/json");
        }

             
        [HttpGet]
        [SiteMapTitle("CategoryName")]
        [SiteMapTitle("category.tb_section.section_title", Target = AttributeTarget.ParentNode)]
        public ActionResult category(int id_category,int pageNumber=1)
        {

            CategoryPageModel page = new CategoryPageModel();
        
            var category = catRepository.GetById(id_category);
            if (category == null)
            {
                throw new HttpException(404, "Ads Category not found");
            }

            page.CategoryName = category.category_title;
            page.category = category;
      
            var premium = annoncesRepository.GetPremiumAds().ToList();
            if (premium == null)
                throw new HttpException(404, "Premium Ads not found");
            if (premium != null)
            {
                page.AdsPremium = premium;
            }

            var result = annoncesRepository.GetAdsByCategory(id_category);

            if (result == null)
            { throw new HttpException(404, "Ads Category not found"); }

                ViewBag.id_commune = new SelectList(communeRepository.GetAllCommune().OrderBy(x => x.commune), "id_commune", "commune");
                ViewBag.ad_user_type = null;
                ViewBag.id_category = this.populateCategoryBySection();

                if (result != null)
                {
                    page.AdsList = result.ToPagedList(pageNumber,10);
                }

                return View(page);
           
        }


        [HttpGet]
        [SiteMapTitle("CategoryName")]
        public ActionResult section(int id_section,int pageNumber)
        {

            CategoryPageModel page = new CategoryPageModel();

            var section = secRepository.GetById(id_section);
            if (section == null)
            {
                throw new HttpException(404, "Ads Category not found");
            }

            ViewBag.CategoryName = section.section_title;

            var premium = annoncesRepository.GetPremiumAds().ToList();
            if (premium == null)
                throw new HttpException(404, "Premium Ads not found");
            if (premium != null)
            {
                page.AdsPremium = premium;
            }

            var result = annoncesRepository.GetAdsBySection(id_section);

            if (result == null)
            { throw new HttpException(404, "Ads Section not found"); }

            ViewBag.id_commune = new SelectList(communeRepository.GetAll(), "id_commune", "commune");
            ViewBag.ad_user_type = null;
            ViewBag.id_category = this.populateCategoryBySection();

            if (result != null)
            {
                page.AdsList = result.ToPagedList(pageNumber,10);
            }

            return View("category",page);

        }

        [HttpGet]
        public ActionResult recherche(string typeannonce, String keywords, int? id_category,int? id_section,int? ad_city,int? id_account_type,string sortOrder,int display = 1,int pageNumber=1)
        {

            CategoryPageModel page = new CategoryPageModel();

            if (id_category != null)
            {
                var category = catRepository.GetById(id_category.GetValueOrDefault());
                if (category == null)
                {
                    throw new HttpException(404, "Ads Category not found");
                }

                page.CategoryName = category.category_title;
                page.category = category;

            }
            else
            {
                page.CategoryName = "Recherche";
            }

    

            var premium = annoncesRepository.GetPremiumAds().ToList();
            if (premium == null)
            { throw new HttpException(404, "Premium Ads not found"); }
           
            if (premium != null)
            {   
                page.AdsPremium = premium;
            }

            if (!String.IsNullOrEmpty(typeannonce))
            {
                ViewBag.currentType = typeannonce;
            }

            if (!String.IsNullOrEmpty(keywords))
            {
                ViewBag.currentKeyword = keywords;
            }

            if (id_category != null)
            {
                ViewBag.currentCategory = id_category;
            }

            if (id_account_type != null)
            {
                ViewBag.currentAccountType = id_account_type;
            }

            if (ad_city != null)
            {
                ViewBag.currentAdCity = ad_city;
            }


            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date asc" : "";
            ViewBag.PrixSortParm = sortOrder == "Prix" ? "Prix desc" : "Prix";

            var result = annoncesRepository.SearchAds(typeannonce, keywords, id_category, id_section,ad_city, id_account_type, sortOrder);
            var total = result.Count(); 
            int pageSize = 12;

            if(result !=null) {
                page.AdsList = result.ToPagedList(pageNumber, pageSize);
                }

            ViewBag.count_annonce = total;
           // ViewBag.ad_city = new SelectList(communeRepository.GetAllCommune().OrderBy(x => x.commune), "id_commune", "commune");
            ViewBag.ad_city = populateCommunetByDepartement();
            ViewBag.ad_user_type = null;
            ViewBag.id_category = this.populateCategoryBySection();
            ViewBag.typeannonce = this.populateTypeAnnonce();
            ViewBag.currentDisplay = display;

            ViewBag.page_description = "Toutokazz, le site de petites annonces  idéal en Haïti  pour vendre, acheter et faire la promotion de vos produits et  service  sur internet";
            ViewBag.ImageUrl = Url.Content("~/Content/img/slides/slide-particulier-solution.jpg");

           return PartialView("_listing",page.AdsList);               
        }

        [HttpGet]
        public ActionResult allannonce(string typeannonce, String keywords, string category_title, int? id_category, string section_title, int? id_section, int? ad_city, int? id_account_type,string  sortOrder,int display = 1,int pageNumber = 1)
        {

            CategoryPageModel page = new CategoryPageModel();
          
            if (id_category != null)
            {
                page.category_view = 2;
                var category = catRepository.GetById(id_category.GetValueOrDefault());
                if (category == null)
                {
                    throw new HttpException(404, "Ads Category not found");
                }

               var section = secRepository.GetById(category.id_section.GetValueOrDefault());
                    if (section != null)
                    {
                        page.selected_section = section;
                    }
                
                page.CategoryName = category.category_title;
                ViewBag.category_title = category.category_title;
                page.category = category;
                ViewBag.Title = "Toutokazz - annonces " + category.category_title ;
            }
            else if (id_section != null)
            {

                page.category_view = 2;
                var section  = secRepository.GetById(id_section.GetValueOrDefault());

                    if(section !=null) {
                        page.CategoryName = section.section_title;
                        ViewBag.section_title = section.section_title;
                        ViewBag.Title = "Toutokazz - annonces " + section.section_title;

                        page.selected_section = section;
                    }
                

             }
             else 
            {
                IEnumerable<tb_section> sectionList = secRepository.GetAll();

                page.CategoryName = "Toutes les Annonces";
                
                page.category_view = 1;
                page.selected_section = null;
                page.category = null;
                ViewBag.Title = "Toutokazz - Toutes les annonces - ";

                page.categoryList = sectionList;
            }

            var villes = communeRepository.GetAllCommune();
            page.communeList = villes;

            var premium = annoncesRepository.GetPremiumAds().ToList();
            if (premium == null)
            { throw new HttpException(404, "Premium Ads not found"); }

            if (premium != null)
            {
                page.AdsPremium = premium;
            }
            
            if(!String.IsNullOrEmpty(typeannonce)) {
                ViewBag.currentType = typeannonce;
            }

            if (!String.IsNullOrEmpty(keywords))
            {
                ViewBag.currentKeyword = keywords;
            }

            if (id_category!=null)
            {
                ViewBag.currentCategory = id_category;
            }

            if (id_account_type != null)
            {
                ViewBag.currentAccountType = id_account_type;
            }

            if (ad_city != null)
            {
                ViewBag.currentAdCity = ad_city;
                page.SelectedCity = communeRepository.GetById(ad_city.GetValueOrDefault()).commune;
            }
            else { page.SelectedCity = "Haiti"; }
            
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date asc" : "";
            ViewBag.PrixSortParm = sortOrder == "Prix" ? "Prix desc" : "Prix";

            var result = annoncesRepository.SearchAds(typeannonce, keywords, id_category, id_section,ad_city, id_account_type,sortOrder);
            var total = result.Count();
            int pageSize = 12;

            if (result != null)
            {
                page.AdsList = result.ToPagedList(pageNumber, pageSize);
            }



            ViewBag.count_annonce = total;
           // ViewBag.ad_city = new SelectList(communeRepository.GetAllCommune().OrderBy(x => x.commune), "id_commune", "commune");
            ViewBag.ad_city = populateCommunetByDepartement();
            ViewBag.ad_user_type = null;
            ViewBag.id_category = this.populateCategoryBySection();
            ViewBag.typeannonce = this.populateTypeAnnonce();
            ViewBag.currentDisplay = display;

            return View(page);
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

        private List<object> JsonPopulateCategoryBySection()
        {


            IEnumerable<tb_section> section = secRepository.GetAll();
            IEnumerable<tb_category> category = catRepository.GetAll();

             List<object> categoryBySection = new List<object>();
            foreach (var sec in section)
            {
                var catList = new List<object>();
                foreach (var cat in category.Where(c => c.id_section == sec.id_section))
                {
                    catList.Add(new  { id_category = cat.id_category.ToString(), category_title = cat.category_title });
                }
                categoryBySection.Add(new { id_section = sec.id_section, section_title = sec.section_title, catList });
            }

            return  categoryBySection;

        }


        private IDictionary<string, IEnumerable<SelectListItem>> populateCommunetByDepartement()
        {


            IEnumerable<tb_departement> dep = communeRepository.GetAllDepartement();
            IEnumerable<tb_commune> com = communeRepository.GetDistrictCommune();

            IDictionary<string, IEnumerable<SelectListItem>> CommuneByDepartement = new Dictionary<string, IEnumerable<SelectListItem>>();
            foreach (var item in dep)
            {
                var comList = new List<SelectListItem>();
                foreach (var city in com.Where(c => c.id_departement == item.id_departement))
                {
                    comList.Add(new SelectListItem { Value = city.id_commune.ToString(), Text = city.commune });
                }
                CommuneByDepartement.Add(item.departement, comList.OrderBy(x=>x.Text));
            }

            return CommuneByDepartement;

        }

        [HttpGet]
        [SiteMapTitle("tb_category.category_title", Target = AttributeTarget.ParentNode)]
        public ActionResult details(int id,string ad_title)
        {
            var annonce = annoncesRepository.GetById(id);
        
            if( annonce == null) {
                throw new HttpException(404, "Annonce not found");
            }
            ViewBag.id_section = annonce.id_section;
            ViewBag.section_title = secRepository.GetById((int)annonce.id_section).section_title;
            ViewBag.category_title = catRepository.GetById((int)annonce.id_category).category_title;
            ViewBag.Title = "Toutokazz - annonces " + annonce.tb_category.category_title +"-"+ ad_title;
            ViewBag.description = annonce.ad_description;
            ViewBag.keywords = "toutokazz, toutokaz,haiti,annonces, site annonces,déposer,publiez," + annonce.tb_category.category_title;
            ViewBag.page_description = annonce.ad_description;
            ViewBag.ImageUrl = Url.Content(WebConfigurationManager.AppSettings["PhotosDir"] + annonce.tb_ad_image.FirstOrDefault().image_filename);
            return View(annonce);
        }

        [HttpGet]
        public ActionResult annonceur(int id,string annonceurname,int pageNumber = 1)
        {
            AnnonceurViewModel page = new AnnonceurViewModel();

            var annonceur = accRepository.GetById(id);
            if (annonceur == null)
            {
                throw new HttpException(404, "Annonce not found");
            }

            page.AnnonceurName = annonceurname;
            page.annoneur = annonceur;

            var result  = annoncesRepository.GetAdsByAnnonceur(id);
            if (result == null)
            {
                throw new HttpException(404, "Annonce not found");
            }
            var total = result.Count();
            int pageSize = 12;

            if (result != null)
            {
                page.AdsList = result.ToPagedList(pageNumber, pageSize);
            }
            ViewBag.count_annonce = total;
           
   

            return View(page);
        }

        [HttpGet]
        public ActionResult vitrine(int id, string annonceurname, int pageNumber = 1)
        {
            AnnonceurViewModel page = new AnnonceurViewModel();

            var annonceur = accRepository.GetById(id);
            if (annonceur == null)
            {
                throw new HttpException(404, "Annonce not found");
            }

            page.AnnonceurName = annonceurname;
            page.annoneur = annonceur;

            var result = annoncesRepository.GetAdsByAnnonceur(id);
            if (result == null)
            {
                throw new HttpException(404, "Annonce not found");
            }
            var total = result.Count();
            int pageSize = 12;

            if (result != null)
            {
                page.AdsList = result.ToPagedList(pageNumber, pageSize);
            }
            ViewBag.count_annonce = total;

            ViewBag.page_description = "Toutokazz, le site de petites annonces  idéal en Haïti  pour vendre, acheter et faire la promotion de vos produits et  service  sur internet";
            ViewBag.ImageUrl = Url.Content("~/Content/img/slides/slide-positive-women.jpg");
            return View(page);
        }

        [HttpPost]
        public JsonResult contact(ContacterViewModel contact)
        {
            if(ModelState.IsValid) {

                try
                {
                    AnnonceMailer.ContactAnnonceur(contact).Send();
                    AnnonceMailer.ContactVisiteurConfirmation(contact).Send();
                    return Json("Message envoye.", JsonRequestBehavior.AllowGet);

                }
                catch (Exception exp)
                {

                    throw new HttpException(404, exp.Message);
                }
            }
            return Json("Une Erreur ou La forme n'est pas valide, verifier les champs obligatoires.", JsonRequestBehavior.AllowGet);
            
        }
        
        [ChildActionOnly]
        public ActionResult GetCategoryMenu()
        {
            IEnumerable<tb_section> menuList = secRepository.GetAll();

            if (menuList != null)
            {
                var section = (IList<tb_section>)menuList;// should not be cast
                return PartialView("_CategoriesMenu", section);

            }
            else
            {
                return HttpNotFound("Can not load category menu");
            }

        }
    
        [ChildActionOnly]
        public ActionResult GetCategory(int index)
        {
            var q = secRepository.GetCategoryBySectionId(index);

            return PartialView("_Categories", q.ToList());
        }

        public ActionResult ListCategory(int index)
        {
            var q = secRepository.GetCategoryBySectionId(index);

            return View("_Categories", q.ToList());
        }

          [ChildActionOnly]
        public ActionResult GetNouvellesAnnonces()
        {
            var newAds = annoncesRepository.GetNewAds().ToList();
            if (newAds == null)
            { throw new HttpException(404, "New Ads not found"); }
            else
            {
                return PartialView("_NouvellesAnnonces", newAds);
            }

        }


        public ActionResult _GetSideCategory(int? id_section,int? id_category,int? ad_city )
        {

            return PartialView("_asideCategory");
        }

          [ChildActionOnly]
        public ActionResult _GetSearchAds()
          {
             // ViewBag.ad_city = new SelectList(communeRepository.GetAll(), "id_commune", "commune");
              ViewBag.ad_city =populateCommunetByDepartement();
              ViewBag.ad_user_type = null;
              ViewBag.id_category = this.populateCategoryBySection();
              ViewBag.typeannonce = this.populateTypeAnnonce();
              return PartialView("_SearchKeywords");
          }

          public ActionResult SearchKeywords()
          {

              ViewBag.ad_city = populateCommunetByDepartement();
              ViewBag.ad_user_type = null;
              ViewBag.id_category = this.populateCategoryBySection();
              ViewBag.typeannonce = this.populateTypeAnnonce();
              return View("_SearchKeywords");
          }


        [ChildActionOnly]
        public ActionResult _GetSimilarAds(int id)
          {
              var result = annoncesRepository.GetSimilarAds(id);
              if (result == null)
              {
                  throw new HttpException(404, "Annonce not found");
              }
              return PartialView("_annoncesSimilaires",result);
          }

        public SelectList populateTypeAnnonce()
          {
              return new SelectList(new[] {
                  new {Id = "avendre", Value = "Vendre"},
                  new { Id = "acheter", Value ="Acheter"}
              },"Id","Value");
          }
      
        [HttpGet]
        public JsonResult getListcommune(string searchTerm, int pageSize, int pageNum)
          {
             IEnumerable<tb_commune> communes = communeRepository.GetSelectedCommune(searchTerm,pageSize, pageNum);
             int nbCommune = communeRepository.GetSelectedCommuneCount(searchTerm, pageSize, pageNum);

             return Json(communes,JsonRequestBehavior.AllowGet);


          }

        [HttpGet]
        public JsonResult getCommuneByDepartement(int id)
        {
            IEnumerable<tb_commune> communes = communeRepository.GetCommuneByDepartement(id);
      
            return Json( new SelectList(communes.ToArray(),"id_commune","commune"), JsonRequestBehavior.AllowGet);

        }

        public  ActionResult getData(){      

            return Json(JsonPopulateCategoryBySection(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getJSONData()
        {
            var data = new List<object>();
            IEnumerable<tb_departement> departments = communeRepository.GetAllDepartement();
            IEnumerable<tb_commune> communes = communeRepository.GetDistrictCommune().OrderBy(x => x.order_commune).ToList();
            IEnumerable<tb_ad_reference> transmission = annoncesRepository.GetRefItems("transmission");

          //  data.Add(sectionList.Select(x => new { id_section = x.id_section ,section_title = x.section_title,icon_image = x.icon_image }));
           // data.Add(categoryList.Select(x => new { id_category = x.id_category, id_section=x.id_section, category_title = x.category_title }));
            data.Add( new { ad_category = JsonPopulateCategoryBySection()});
            data.Add( new { departement = departments.Select(x => new { id_departement = x.id_departement, departement = x.departement }) });
            data.Add( new { commune = communes.Select(x => new { id_commune = x.id_commune, id_departement = x.id_departement, commune = x.commune }) });
            data.Add( new { transmission = transmission.Select(x => new { ref_item_id = x.ref_item_id, ref_item = x.ref_item})});
            return Json( data, JsonRequestBehavior.AllowGet);

        }

        [ChildActionOnly]
        public bool isPhotoListNullorEmpty(IEnumerable<UploadFileModel> images){
            
            bool result = true;
            
            if(images!=null){                
                foreach(var file in images){                  
                    if(file!= null){
                        if(file.image != null){
                            result = false;
                            return result;
                        }
                    }
                }
            }
            return result;

        }

        public ActionResult EmailLayout()
        {
            ViewBag.Message = "Bonjour, J'aimerais avoir plus de renseignements sur votre annonce déposée sur toutokazz.com.Merci de me contacter à cet effet. Cordialement";
            ViewBag.Email = "mayerantoine@gmail.com";
            ViewBag.Telephone = "5094410571";
            ViewBag.annonceCode = "A20141212200";
            return View();
        }

        public ActionResult publiezannonce()
        {
            HomePageModel model = new HomePageModel();
            ViewBag.Title = "Toutokazz - Deposer  des annonces gratuites";
            model.sectionList = secRepository.GetAll().OrderBy(x => x.section_order).ToList();
            return View(model);
        }

        public ActionResult _VillesPrincipales()
        {
            var result = communeRepository.GetDistrictCommune();
            return PartialView("_VillesPrincipales", result.ToList())
;
        }
      
        public ActionResult BadAction()
          {
              throw new Exception("You forgot to implement this action!");
          }

        public ActionResult ServerPath()
          {
             String serverpath1 = Server.MapPath("..");
             String serverpath2 = Server.MapPath("/");
             String serverpath3 = Server.MapPath("~");
             String serverpath4 = Server.MapPath(".\\");


             ViewBag.serverpath1 = serverpath1;
             ViewBag.serverpath2 = serverpath2;
             ViewBag.serverpath3 = serverpath3;
             ViewBag.serverpath4 = serverpath4;

                return View();
          }


        public ActionResult kdeposerannonce()
        {
            return View("k_deposerannonce");
        }
    
    }
}
