using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Security;

namespace Toutokaz.WebUI.Security
{
    public interface IAuthProvider
    {
        bool AuthenticateMembership(string email, string password);
        bool Authenticate(string email, string password);
        MembershipCreateStatus CreateUserMembership(string nom, string prenom, string password, string email);
        string CreateUser(string nom, string prenom, string password, string email);
        string CreateUserNoToken(string nom, string prenom, string password, string email);
        MembershipCreateStatus CreateUserProMembership(string nom, string prenom,string nom_entrprise, string password, string email);
        string CreateUserPro(string nom, string prenom, string nom_entrprise, string password, string email);
        string CreateUserProNoToken(string nom, string prenom, string nom_entrprise, string password, string email);
        bool ChangeUserPasswordMembership(String username, String currentPassword, String newPassword);
        bool ChangeUserPassword(String username, String currentPassword, String newPassword);
        IEnumerable<MembershipUser> GetAllUser();
    }
}
