using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToutokazAdmin.WebUI.Models
{
    public class AnnoncesViewModel
    {

        public int id_ad { get; set; }
        public string category { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string image_path { get; set; }
        public string ad_description { get; set; }
        public string ad_title { get; set; }
        public int? ad_status { get; set; }
        public int? ad_is_published { get; set; }
        public System.DateTime? ad_date_expired { get; set; }
        public System.DateTime? ad_date_created { get; set; }

      
    }
}