using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Toutokaz.WebUI.Models
{
    public class UploadFilesResultModel
    {
        public String Name { get; set; }
        public int Length { get; set; }
        public String Type { get; set; }
        public String Error { get; set; }
    }
}