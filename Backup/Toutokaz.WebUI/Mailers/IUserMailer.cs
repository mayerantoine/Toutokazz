using Mvc.Mailer;
using Toutokaz.WebUI.Models;

namespace Toutokaz.WebUI.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage Welcome(string Email);
            MvcMailMessage confirmationEmail(string username, string token);
			MvcMailMessage PasswordReset(string email,string token);

	}
}