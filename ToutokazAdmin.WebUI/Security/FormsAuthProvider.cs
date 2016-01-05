using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Security;
using Toutokaz.Domain.Models;

namespace ToutokazAdmin.WebUI.Security
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string email, string password)
        {
            bool result  = Membership.ValidateUser(email,password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(email, false);
            }
            return result;            
            
            
        }

        public MembershipCreateStatus CreateUser(string nom, string prenom,string password,string email)
        {
            MembershipCreateStatus createStatus;
            try
            {
              
                // MembershipUser newuser  = Membership.CreateUser(email, password, email);
               MembershipUser newuser = Membership.CreateUser(email, password, email, passwordQuestion: null, passwordAnswer: null, isApproved: true, 
                    providerUserKey: null,status: out createStatus);
                
                if (createStatus == MembershipCreateStatus.Success) {
  
               
                    using(toutokaz_dbEntities ctx = new toutokaz_dbEntities()){

                        tb_account p = new tb_account
                        {
                            id_user = (Guid)newuser.ProviderUserKey,
                            firstname = prenom,
                            lastname = nom,
                            email = email,
                            username = email,
                            id_account_type = 1,
                            status = "active",
                            

                        };

                        ctx.tb_account.Add(p);
                        ctx.SaveChanges();
                    }
                }

            }
            catch(MembershipCreateUserException exp){
                throw exp; 
            }

            return createStatus;
        }

        public MembershipCreateStatus CreateUserPro(string nom, string prenom,  string nom_entreprise,string password, string email)
        {
            MembershipCreateStatus createStatus;
            try
            {

                // MembershipUser newuser  = Membership.CreateUser(email, password, email);
                MembershipUser newuser = Membership.CreateUser(email, password, email, passwordQuestion: null, passwordAnswer: null, isApproved: true,
                     providerUserKey: null, status: out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {


                    using (toutokaz_dbEntities ctx = new toutokaz_dbEntities())
                    {

                        tb_account p = new tb_account
                        {
                            id_user = (Guid)newuser.ProviderUserKey,
                            firstname = prenom,
                            lastname = nom,
                            email = email,
                            username = email,
                            nom_entreprise =nom_entreprise,
                            id_account_type = 2,
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
    }
}
