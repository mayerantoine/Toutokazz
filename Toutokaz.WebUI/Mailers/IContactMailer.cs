using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toutokaz.Domain.Models;

namespace Toutokaz.WebUI.Mailers
{
    public  interface IContactMailer
    {

         MvcMailMessage ContactToutokazz(tb_contact contact);
    }
}
