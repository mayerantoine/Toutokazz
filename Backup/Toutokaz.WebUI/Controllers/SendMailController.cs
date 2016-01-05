using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Mvc;
using Mvc.Mailer;
using Toutokaz.WebUI.Models;
using Toutokaz.WebUI.Mailers;

namespace Toutokaz.WebUI.Controllers
{
    public class SendMailController : Controller
    {

        private IUserMailer _userMailer = new UserMailer();
        public IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }
        //
        // GET: /SendMail/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendWelcomeMessage() {
            try
            {
                UserMailer.Welcome("mayerantoine@gmail.com").Send();
            }
            catch(Exception exp){

                throw new HttpException(404,exp.Message);
            }
            return RedirectToAction("Index");
        }
        
         [HttpGet]
        public ActionResult Send(MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.From);
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("mayerantoine@gmail.com", "Sup3rStr0ngP@ss");// Enter seders User name and password
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return View("Index", _objModelMail);
            }
            else
            {
                return View();
            }
        }

    }
}
