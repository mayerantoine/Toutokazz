using Mvc.Mailer;
using Toutokaz.WebUI.Models;

namespace Toutokaz.WebUI.Mailers
{ 
    public class AnnonceMailer : MailerBase, IAnnonceMailer 	
	{
		public AnnonceMailer()
		{
			MasterName="_LayoutMail";
		}
		
		public virtual MvcMailMessage ContactAnnonceur(ContacterViewModel contact)
		{
			ViewBag.Message = contact.message;
            ViewBag.Email = contact.Email;
            ViewBag.Telephone = contact.telephone;
            ViewBag.annonceId = contact.idad;
            ViewBag.adTitle = contact.ad_title;
            ViewBag.annonceCode= contact.ad_code;
			return Populate(x =>
			{
				x.Subject = "Toutokazz- Message concernant votre annonce";
				x.ViewName = "ContactAnnonceur";
				x.To.Add("equipe@toutokazz.com");
                x.To.Add(contact.Emailaccount);
                x.Body = contact.message;
               			});
		}

        public virtual MvcMailMessage ContactVisiteurConfirmation(ContacterViewModel contact)
		{
            ViewBag.Message = contact.message;
            ViewBag.Email = contact.Email;
            ViewBag.Telephone = contact.telephone;
            ViewBag.annonceId = contact.idad;
            ViewBag.annonceCode = contact.ad_code;
            ViewBag.adTitle = contact.ad_title;
            return Populate(x =>
            {
                x.Subject = "Toutokazz- message de confirmation";
                x.ViewName = "ContactVisiteurConfirmation";
                x.To.Add("equipe@toutokazz.com");
                x.To.Add(contact.Email);
                x.Body = contact.message;
            });
		}

        public virtual MvcMailMessage deposerannonce(string username, string titre, string description)
        {
            ViewBag.username = username;
            ViewBag.titre = titre;
            ViewBag.description = description;
            return Populate(x =>
            {
                x.Subject = "Toutokazz - Annonce deposer";
                x.ViewName = "depotAnnonce";
                x.To.Add("equipe@toutokazz.com");
                x.To.Add("mayerantoine@gmail.com");
                x.Body = "";
            });
        }
 	}
}