using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Toutokaz.Domain.Models;
using Toutokaz.WebUI.Mailers;
using Toutokaz.WebUI.Models;
using Toutokaz.WebUI.Security;
using WebMatrix.WebData;

namespace Toutokaz.WebUI.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider authProvider;
        IAccountRepository account;

        private IUserMailer _welcomeMailer = new UserMailer();
        public IUserMailer WelcomeMailer
        {
            get { return _welcomeMailer; }
            set { _welcomeMailer = value; }
        }
        

        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
            account = new AccountRepository();
        }

        //
        // GET: /Account/

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Moncompte()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                if (authProvider.Authenticate(login.Email, login.Password))
                {
                    FormsAuthentication.SetAuthCookie(login.Email, false);
                  

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        String role  = Roles.GetRolesForUser(login.Email).SingleOrDefault();

                        if (role.Equals("administrateur"))
                        { return RedirectToAction("Index", "Home", new { area = "Admin" }); }
                        else
                        {
                            return RedirectToAction("Index", "MesAnnonces");
                        }
              
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
            WebSecurity.Logout();
            return RedirectToAction("Moncompte", "Account", null);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CreateLandingOne()
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
                   string c;
                    if( model.account_type == 1 && String.IsNullOrEmpty(model.nom_entreprise)){
                         c = authProvider.CreateUser(model.Nom, model.Prenom, model.password, model.Email);
                    }
                    else
                    {
                        c = authProvider.CreateUserPro(model.Nom, model.Prenom, model.nom_entreprise, model.password, model.Email);                       
                    }
                    //Succesfull page
                    if (!String.IsNullOrEmpty(c))
                    {
                        WelcomeMailer.confirmationEmail(model.Email,c).Send();

                    /*    if (authProvider.Authenticate(model.Email, model.password))
                        {
                            FormsAuthentication.SetAuthCookie(model.Email, false);
                             return RedirectToAction("index", "mesannonces");
                        }*/
                        return RedirectToAction("RegistrationSuccess", "Account");
                     
                    }
                    else
                    {
                        @ViewBag.Message = "<div class=\"alert alert-danger\">Il y a erreur lors de la creation de votre compte</div>";
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
                        WelcomeMailer.confirmationEmail(model.Email, c).Send();

                        /*    if (authProvider.Authenticate(model.Email, model.password))
                            {
                                FormsAuthentication.SetAuthCookie(model.Email, false);
                                 return RedirectToAction("index", "mesannonces");
                            }*/
                        return RedirectToAction("RegistrationSuccess", "Account");

                    }
                    else
                    {
                        @ViewBag.Message = "<div class=\"alert alert-danger\">Il y a erreur lors de la creation de votre compte</div>";
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

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreatePro(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string c;
                    c = authProvider.CreateUserPro(model.Nom, model.Prenom,model.nom_entreprise, model.password, model.Email);
                    //Succesfull page
                    if (!String.IsNullOrEmpty(c))
                    {

                        WelcomeMailer.confirmationEmail(model.Email, c).Send();
                      /*  if (authProvider.Authenticate(model.Email, model.password))
                        {
                            FormsAuthentication.SetAuthCookie(model.Email, false);
                            return RedirectToAction("index", "mesannonces");
                        }*/

                        return RedirectToAction("RegistrationSuccess", "Account");
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
        public ActionResult RegistrationSuccess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Registration(string token)
        {
            if (WebSecurity.ConfirmAccount(token))
            {
                return RedirectToAction("ConfirmationSuccess");
            }
            return RedirectToAction("ConfirmationFailure");
            
  
        }

        [HttpGet]
        public ActionResult ConfirmationSuccess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConfirmationFailure()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(String currentPassword,String newPassword)
        {
            try
            {
                bool c = authProvider.ChangeUserPassword(User.Identity.Name, currentPassword, newPassword);
                if (c)
                {
                    return View("ChangePasswordSuccess");
                }
            }
            catch(InvalidOperationException exp) {

                @ViewBag.Message = "<div class=\"alert alert-danger\">Il y a erreur lors du changement de mot de passe</div>";

            }
            @ViewBag.Message = "<div class=\"alert alert-danger\">Il y a erreur lors du changement de mot de passe</div>";
            return View("ChangePassword");
        }

        [HttpGet]
        public ActionResult EditProfile() {
            if (Request.IsAuthenticated)
            {
                string username = User.Identity.Name;
                int UserId = WebSecurity.GetUserId(username);
              //  MembershipUser user =  Membership.GetUser(username);

                tb_account acc = account.GetAccountByUserId(UserId);
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

            return RedirectToAction("Index", "home");
        
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {
           int UserId = 0;
            try
            {
                // validate email
              //  UserId = WebSecurity.GetUserId(email);
               
                
                if (!WebSecurity.UserExists(email))
                {
                    @ViewBag.Message = "<div class=\"alert alert-danger\">Votre compte n'est pas valide</div>";
                    return View();
                }

                UserId = WebSecurity.GetUserId(email);
                if (!OAuthWebSecurity.HasLocalAccount(UserId))
                {
                    @ViewBag.Message = "<div class=\"alert alert-danger\">Votre compte est un compte externe. Vous ne pouvez pas changer votre password sur notre site.</div>";
                    return View();
                }

                // generate token
                string token = WebSecurity.GeneratePasswordResetToken(email, 1440);

                //send email
                _welcomeMailer.PasswordReset(email, token).Send();
                
            }
            catch(InvalidOperationException exp) 
            {
                throw exp;
            }
            catch(Exception exp){
                @ViewBag.Message = "<div class=\"alert alert-danger\">Une erreur a été pendant la réinitialisation de votre mot de passe.</div>";
                return View();
            }

            @ViewBag.Message = "<div class=\"alert alert-danger\">La réinitialisation de votre mot de passe a bien été prise en compte.Nous avons envoyé par courrier électronique des instructions pour configurer votre mot de passe à l'adresse e-mail que vous avez soumis. Vous devriez le recevoir sous peu.</div>";
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword(string token)
        {
            ViewBag.token = token;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string token,string Password,string ConfirmPassword)
        {
            bool c = WebSecurity.ResetPassword(token, Password);

            if(c) {
                return RedirectToAction("ResetPasswordComplete", "account");
            }

            @ViewBag.Message = "<div class=\"alert alert-danger\">Il y a erreur lors de la reinitialisation de votre compte</div>";
            return View();
        }

        [HttpGet]
        public ActionResult ResetPasswordComplete(){

            return View();
        }


        [HttpPost]
        public ActionResult EditProfile(ProfileViewModel model)
        {
            if(ModelState.IsValid){
                tb_account acc  = account.GetById(model.id_account);
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
                catch(Exception exp)
                {
                    throw exp;
                }
                return RedirectToAction("index", "mesannonces");
            }
            
            return View(model);

        }


        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginButton(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginButton", OAuthWebSecurity.RegisteredClientData);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginButtonBig(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginButtonb", OAuthWebSecurity.RegisteredClientData);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { Email = result.UserName, ExternalLoginID = loginData});
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginID,out provider, out providerUserId))
            {
                return RedirectToAction("index","mesannonces");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                     // Insert a new user into the database
                using (toutokaz_dbEntities db = new toutokaz_dbEntities())
                {
                    tb_user_profile user = db.tb_user_profile.FirstOrDefault(u => u.username.ToLower() == model.Email.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.tb_user_profile.Add(new tb_user_profile { username = model.Email });
                        db.SaveChanges();

                        // Insert name into account table
                        int Id = WebSecurity.GetUserId(model.Email);
                        db.tb_account.Add(new tb_account
                        {
                            UserId = Id,
                            firstname = model.Prenom,
                            lastname = model.Nom,
                            email = model.Email,
                            username = model.Email,
                            id_account_type = 1,
                            role = "annonceur",
                            status = "active",
                        });
                        db.SaveChanges();
                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.Email);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }


        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("index", "mesannonces");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    
    }
}

