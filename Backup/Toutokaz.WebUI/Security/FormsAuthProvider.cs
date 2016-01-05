using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Security;
using Toutokaz.Domain.Models;
using WebMatrix.Data;
using WebMatrix.WebData;
using System.Web.Mvc;

namespace Toutokaz.WebUI.Security
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool AuthenticateMembership(string email, string password)
        {
            bool result = Membership.ValidateUser(email, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(email, false);
            }
            return result;


        }

        public bool Authenticate(string email, string password)
        {
            bool result = WebSecurity.Login(email, password);
            if (result)
            {
                // Get roles
                string current = Roles.GetRolesForUser(email).FirstOrDefault();
                int UserId = WebSecurity.GetUserId(email);
                FormsAuthentication.SetAuthCookie(email, false);
            }
            return result;


        }

        public MembershipCreateStatus CreateUserMembership(string nom, string prenom, string password, string email)
        {
            MembershipCreateStatus createStatus;
            try
            {

                // MembershipUser newuser  = Membership.CreateUser(email, password, email);
                MembershipUser newuser = Membership.CreateUser(email, password, email, passwordQuestion: null, passwordAnswer: null, isApproved: true,
                     providerUserKey: null, status: out createStatus);
            
                if (createStatus == MembershipCreateStatus.Success)
                {
                    try
                    {
                        Roles.AddUserToRole(newuser.UserName, "annonceur");
                    }
                    catch (System.Configuration.Provider.ProviderException exp)
                    {
                        throw exp;
                    }

                    using (toutokaz_dbEntities ctx = new toutokaz_dbEntities())
                    {

                        tb_account p = new tb_account
                        {
                            id_user = (Guid)newuser.ProviderUserKey,
                            firstname = prenom,
                            lastname = nom,
                            email = email,
                            username = email,
                            id_account_type = 1,
                            role = "annonceur",
                            status = "active",


                        };

                        ctx.tb_account.Add(p);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (MembershipCreateUserException exp)
            {
                throw exp;
            }

            return createStatus;
        }

       [AllowAnonymous]
        public string CreateUser(string nom, string prenom, string password, string email)
        {
            string token  = null;
            try
            {
                // create account and profile
                 token = WebSecurity.CreateUserAndAccount(email, password,null,true);
                try
                {
                    Roles.AddUserToRole(email, "annonceur");
                }
                catch (System.Configuration.Provider.ProviderException exp)
                {
                    throw exp;
                }

                using (toutokaz_dbEntities ctx = new toutokaz_dbEntities())
                {
                    int Id = WebSecurity.GetUserId(email);
                    tb_account p = new tb_account
                    {
                        UserId = Id,
                        firstname = prenom,
                        lastname = nom,
                        email = email,
                        username = email,
                        id_account_type = 1,
                        role = "annonceur",
                        status = "active",


                    };

                    ctx.tb_account.Add(p);
                    ctx.SaveChanges();
                    
                }


            }
            catch (MembershipCreateUserException exp)
            {
                throw exp;
            }
            catch (InvalidOperationException exp)
            {
                throw exp;
            }

            return token;
        }
             
        public MembershipCreateStatus CreateUserProMembership(string nom, string prenom, string nom_entreprise, string password, string email)
        {
            MembershipCreateStatus createStatus;
            try
            {

                // MembershipUser newuser  = Membership.CreateUser(email, password, email);
                MembershipUser newuser = Membership.CreateUser(email, password, email, passwordQuestion: null, passwordAnswer: null, isApproved: true,
                     providerUserKey: null, status: out createStatus);

               if (createStatus == MembershipCreateStatus.Success)
                {
                    try
                    {
                        Roles.AddUserToRole(newuser.UserName, "annonceur");
                    }
                    catch (System.Configuration.Provider.ProviderException exp)
                    {
                        throw exp;
                    }


                    using (toutokaz_dbEntities ctx = new toutokaz_dbEntities())
                    {

                        tb_account p = new tb_account
                        {
                            id_user = (Guid)newuser.ProviderUserKey,
                            firstname = prenom,
                            lastname = nom,
                            email = email,
                            username = email,
                            nom_entreprise = nom_entreprise,
                            id_account_type = 2,
                            role = "annonceur",
                            status = "active",


                        };

                        ctx.tb_account.Add(p);
                        ctx.SaveChanges();
                    }
                }

            }
            catch (MembershipCreateUserException exp)
            {
                throw exp;
            }

            return createStatus;
        }

        [AllowAnonymous]
        public string CreateUserPro(string nom, string prenom, string nom_entreprise, string password, string email)
        {
            string token = null;
            try
            {


                // create account and profile
               token =  WebSecurity.CreateUserAndAccount(email, password,null, true);


                try
                {
                    Roles.AddUserToRole(email, "annonceur");
                }
                catch (System.Configuration.Provider.ProviderException exp)
                {
                    throw exp;
                }

                using (toutokaz_dbEntities ctx = new toutokaz_dbEntities())
                {
                    int Id = WebSecurity.GetUserId(email);
                    tb_account p = new tb_account
                    {
                        UserId = Id,
                        firstname = prenom,
                        lastname = nom,
                        email = email,
                        username = email,
                        nom_entreprise = nom_entreprise,
                        id_account_type = 2,
                        role = "annonceur",
                        status = "active",


                    };

                    ctx.tb_account.Add(p);
                    ctx.SaveChanges();
        
                }


            }
            catch (MembershipCreateUserException exp)
            {
                throw exp;
            }
            catch (InvalidOperationException exp)
            {
                throw exp;
            }

            return token;
        }
        
        public bool ChangeUserPasswordMembership(String username, String currentPassword, String newPassword)
        {
            MembershipUser user = Membership.GetUser(username);
            if (user.ChangePassword(currentPassword, newPassword))
            {
                return true;
            }
            return false;
        }

        public bool ChangeUserPassword(String username, String currentPassword, String newPassword)
        {
            bool user = WebSecurity.ChangePassword(username, currentPassword, newPassword);
            if (user)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<MembershipUser> GetAllUser()
        {
            IEnumerable<MembershipUser> model = Membership.GetAllUsers().Cast<MembershipUser>().ToList();

           return model;
        }

        
    }
}
