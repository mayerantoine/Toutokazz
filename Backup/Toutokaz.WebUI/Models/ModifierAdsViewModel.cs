using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toutokaz.WebUI.Models
{
    public class ModifierAdsViewModel
    {

        [Required]
        public int id_ad { get; set; }
        public int? id_section { get; set; }

        [Required(ErrorMessage = "La categorie est obligatoire")]
        public int? id_category { get; set; }

        [Required(ErrorMessage = "Le titre de l'annonce est obligatoire")]
        public string ad_title { get; set; }

        [Required(ErrorMessage = "le prix de l'article est obligatoire")]
        public double ad_price { get; set; }

        [Required(ErrorMessage = "La description de l'annonce est obligatoire")]
        public string ad_description { get; set; }

        [Required(ErrorMessage = "La devise est obligatoire")]
        public int id_devise { get; set; }
    }
}