using Mvc.Mailer;

namespace Toutokaz.WebUI.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_LayoutMail";
		}
		
		public virtual MvcMailMessage Welcome(string Email)
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "Welcome";
				x.ViewName = "Welcome";
				x.To.Add("mayerantoine@gmail.com");
                x.Body = "Bienvenue sur Toutokazz";
			});
		}
 
		public virtual MvcMailMessage PasswordReset(string email,string token)
		{
			ViewBag.token = token;
			return Populate(x =>
			{
                x.Subject = "Toutokazz - Mot de passe réinitialisé";
				x.ViewName = "PasswordReset";
                x.To.Add(email);
			});
		}

        public virtual MvcMailMessage confirmationEmail(string username, string token)
        {
            ViewBag.username = username;
            ViewBag.token= token;
            return Populate(x =>
            {
                x.Subject = "Toutokazz - Activation de votre compte";
                x.ViewName = "confirmationEmail";
             //   x.To.Add("mayerantoine@gmail.com");
                x.To.Add(username);
                x.Body = "";
            });
        }


 	}
}