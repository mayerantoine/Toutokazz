using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ToutokazAdmin.WebUI.Models;
using ToutokazAdmin.WebUI.Security;

namespace ToutokazAdmin.WebUI.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider authProvider;

        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        //
        // GET: /Account/
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if(ModelState.IsValid){

                if (authProvider.Authenticate(login.Email, login.Password))
                {
                    FormsAuthentication.SetAuthCookie(login.Email, false);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Votre Email ou  votre mot de passe est incorrect");
                }
            }
            return View(login);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CreatePro()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MembershipCreateStatus c;
                   c =  authProvider.CreateUser(model.Nom,model.Prenom,model.password,model.Email);
                    //Succesfull page
                   if (c == MembershipCreateStatus.Success)
                   {
                       RedirectToAction("CreateSuccess", "Account");
                   }
                   else
                   {
                       @ViewBag.Message = "<div class=\"alert alert-danger\">Il y a erreur lors de la creation de votre compte: " + c.ToString() + "</div>";
                   }
                }
                catch(Exception exp){
                    String message = exp.Message;
                    @ViewBag.Message = "<div class=\"alert alert-danger\">Il y a erreur lors de la creation de votre compte: " + message + "</div>";

                }
            }
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreatePro(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MembershipCreateStatus c;
                    c = authProvider.CreateUser(model.Nom, model.Prenom, model.password, model.Email);
                    //Succesfull page
                    if (c == MembershipCreateStatus.Success)
                    {
                        RedirectToAction("CreateSuccess", "Account");
                    }
                    else
                    {

                        @ViewBag.Message = "<div class=\"alert alert-danger\">Il y a erreur lors de la creation de votre compte: " + c.ToString() + "</div>";
   
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
        public ActionResult CreateSuccess()
        {
            return View();
        }

 
    }
}
