using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Toutokaz.WebUI.Models
{
    public class MailModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }
        public string username { get; set; }
        public string passord { get; set; }

        public MailModel()
        {
 
            this.Host = "mail.toutokazz.com";
            this.Port = 25;
            this.username = "equipe@toutokazz.com";
            this.passord = "okazyon1234";
        }

        public void send(string from, string to, string subject, string body)
        {
            this.From = from;
            this.To = to;
            this.Subject = subject;
            this.Body = body;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(To);
                mail.From = new MailAddress(From);
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = Host;
                smtp.Port = Port;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                (username, passord);// Enter seders User name and password
                smtp.EnableSsl = false;
                smtp.Send(mail);
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }
        }
    }
}