using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Toutokaz.WebUI.Models
{
    public class AnnoncesListModel
    {

        public int id_ad { get; set; }
        public string category { get; set; }
        public int? id_category;
        public string section { get; set; }
        public string commune { get; set; }
        public int? id_commune { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string image_path { get; set; }
        public string image_filename { get; set; }
        public string ad_description { get; set; }
        public string ad_title { get; set; }
        public int? ad_status { get; set; }
        public int? ad_is_published { get; set; }
        public double? ad_price { get; set; }
        public string devise { get; set; }
        public string ad_type { get; set; }
        public int? account_type { get; set; }
        public System.DateTime? ad_date_expired { get; set; }
        public System.DateTime? ad_date_created { get; set; }
    }
}