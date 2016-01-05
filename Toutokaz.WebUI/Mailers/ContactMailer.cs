using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using Toutokaz.WebUI.Models;
using Toutokaz.Domain.Models;

namespace Toutokaz.WebUI.Mailers
{
    public class ContactMailer : MailerBase, IContactMailer 	
    {

       public ContactMailer()
		{
			MasterName="_LayoutMail";
		}

       public virtual MvcMailMessage ContactToutokazz(tb_contact contact)
       {
           ViewBag.Message = contact.message;
           ViewBag.Email = contact.email;
           ViewBag.Telephone = contact.telephone;
           ViewBag.sujet = contact.sujet;
           ViewBag.nom = contact.nom;
           ViewBag.prenom = contact.prenom;
           return Populate(x =>
           {
               x.Subject = "Toutokazz-" + ViewBag.sujet;
               x.ViewName = "ContactToutokazz";
               x.To.Add("equipe@toutokazz.com");
               x.Body = contact.message;
           });
       }
    }
}