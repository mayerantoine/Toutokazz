using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Security;

namespace ToutokazAdmin.WebUI.Security
{
    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
        MembershipCreateStatus CreateUser(string nom, string prenom, string password, string email);
    }
}
