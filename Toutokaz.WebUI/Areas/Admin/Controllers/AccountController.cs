using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Toutokaz.WebUI.Security;
using WebMatrix.WebData;
using PagedList;
using PagedList.Mvc;
using Toutokaz.Domain.Models;
using Toutokaz.WebUI.Models;

namespace Toutokaz.WebUI.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Admin/Account/

        // GET/Admin/userlist
        // GET/Account/extenaluserlist
        // GET/Admin/adduser?admin?moderateur
        // GET/Admin/deleteuser
        // GET/Admin/disableuser
        // GET/Admin/updateaccount
        // GET/Account/DisassociateExternalUser
        // GET/Account/RemoveExternalUser
        //GET/Account/Resetpassword
        

        IAccountRepository accRepository;
        IAuthProvider authProvider;

        public AccountController(IAccountRepository repository)
        {
            accRepository = repository;
            authProvider = new FormsAuthProvider();
        }


        public ActionResult userlist(string email, string nom, string prenom, int? id_account_type, int page = 1)
        {

           if (!Request.IsAuthenticated)
           {
               throw new HttpException(404, "Access Restricted to logged in members");
           }

           string username = User.Identity.Name;
           int UserId = WebSecurity.GetUserId(username);
           string userrole = Roles.GetRolesForUser(username).SingleOrDefault().ToString();

           if (!userrole.Equals("administrateur"))
           {
               throw new HttpException(404, "Access Restricted to admin members");
           }
           ViewBag.id_account_type = PopulateAccountType();

           var query = accRepository.GetAll();

           if (!String.IsNullOrEmpty(email) || !String.IsNullOrEmpty(nom) || !String.IsNullOrEmpty(prenom) || id_account_type != null )
           {
               if (!String.IsNullOrEmpty(email))
               {
                   query = query.Where(c => c.email.Contains(email));
               }

               if (!String.IsNullOrEmpty(nom))
               {
                   query = query.Where(c => c.lastname.Contains(nom));
               }

               if (!String.IsNullOrEmpty(prenom))
               {
                   query = query.Where(c => c.firstname.Contains(prenom));
               }

               if (id_account_type != null)
               {
                   query = query.Where(c => c.id_account_type == id_account_type);

               }

           }

           return View(query.ToPagedList(page,10));
        }

        [HttpGet]
        public ActionResult EditUser(int UserId)
        {
            if(UserId !=null){
             tb_account acc = accRepository.GetAccountByUserId(UserId);
             if (acc == null)
             {
                 throw new HttpException(404, "no members found");
             }

             ViewBag.accountId = acc.id_account;
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
            return RedirectToAction("userlist", "account", new { Area = "Admin" });    
        }

        [HttpPost]
        public ActionResult EditUser(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                tb_account acc = accRepository.GetById(model.id_account);
                acc.firstname = model.Prenom;
                acc.lastname = model.Nom;
                acc.telephone = model.telephone;
                acc.adresse = model.address;
                try
                {
                    accRepository.Update(acc);
                    accRepository.Save();
                }
                catch (Exception exp)
                {
                    throw exp;
                }
                return RedirectToAction("userlist", "account", new { Area="Admin"});
            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateNoToken(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string c;
                    if (model.account_type == 1 && String.IsNullOrEmpty(model.nom_entreprise))
                    {
                        c = authProvider.CreateUserNoToken(model.Nom, model.Prenom, model.password, model.Email);
                    }
                    else
                    {
                        c = authProvider.CreateUserProNoToken(model.Nom, model.Prenom, model.nom_entreprise, model.password, model.Email);
                    }
                    //Succesfull page
                    if (!String.IsNullOrEmpty(c))
                    {
                        // WelcomeMailer.confirmationEmail(model.Email, c).Send();

                        /*    if (authProvider.Authenticate(model.Email, model.password))
                            {
                                FormsAuthentication.SetAuthCookie(model.Email, false);
                                 return RedirectToAction("index", "mesannonces");
                            }*/

                        @ViewBag.Message = "<div class=\"alert alert-danger\">Il y a erreur lors de la creation de votre compte</div>";
            

                    }
                    else
                    {
                        return RedirectToAction("RegistrationSuccess", "Account", new { Area = "Admin" });
                    }
                }
                catch (Exception exp)
                {
                    String message = exp.Message;
                    @ViewBag.Message = "<div class=\"alert alert-danger\">Il y a erreur lors de la creation de votre compte: " + message + "</div>";

                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult RegistrationSuccess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DesactiverUser(int UserId)
        {
            
            return View();
        }
        
        public SelectList PopulateAccountType()
        {
            return new SelectList(new[] {
                new { Id=1 , Value="Particulier"},
                new { Id=2 , Value="Professionnel"}
            }, "Id", "Value");
        }

        public SelectList PopulateRole()
        {
            return new SelectList(new[] {
                new { Id=1 , Value="annonceur"},
                new { Id=2 , Value="administrateur"},
                new { Id =3, Value="moderateur"},
            }, "Value", "Value");
        }



    }
}
