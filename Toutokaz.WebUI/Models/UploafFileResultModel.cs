using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Toutokaz.WebUI.Models
{
    public class UploadFilesResultModel
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public string Error { get; set; }
        public string url { get; set; }
        public string deleteUrl { get; set; }
        public string deleteType { get; set; }
    }
}