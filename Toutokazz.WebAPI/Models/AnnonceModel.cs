using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Toutokazz.WebAPI.Models
{
    public class AnnonceModel
    {
        public int id_ad { get; set; }
        public string ad_title { get; set; }
        public string ad_description { get; set; }
        public string image_filename { get; set; }
        public string commune_name { get; set; }
        public string category_title { get; set; }
        public string devise { get; set; }
        public double? price { get; set; }
        public int?  ad_status { get; set; }
    }
}