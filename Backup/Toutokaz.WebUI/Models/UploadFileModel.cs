using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Toutokaz.Domain.Models;
using Toutokaz.WebUI.Models;

namespace Toutokaz.WebUI.Models
{
    public class UploadFileModel
    {
        [FileSize(2000000)]
        [FileTypes("jpg,jpeg,png,JPG")]
        public HttpPostedFileBase image { get; set; }
    }
}
