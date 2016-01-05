using Mvc.Mailer;
using Toutokaz.WebUI.Models;

namespace Toutokaz.WebUI.Mailers
{ 
    public interface IAnnonceMailer
    {
        MvcMailMessage ContactAnnonceur(ContacterViewModel contact);
        MvcMailMessage ContactVisiteurConfirmation(ContacterViewModel contact);
        MvcMailMessage deposerannonce(string username, string titre, string description);
	}
}