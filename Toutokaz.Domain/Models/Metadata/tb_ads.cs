using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toutokaz.Domain.Models
{
     [MetadataType(typeof(tb_ads_metadata))]
    public partial class tb_ads
    {
        internal class tb_ads_metadata
        {
            [Required]
            public int id_ad { get; set; }
            [Required(ErrorMessage="La section est obligatoire.")]
            public int id_section { get; set; }

            [Required(ErrorMessage = "La categorie est obligatoire")]
            public int id_category { get; set; }
            
            public int id_user { get; set; }
            [Required(ErrorMessage = "Le titre de l'annonce est obligatoire")]
            public string ad_title { get; set; }

            public string ad_code { get; set; }
            [Required(ErrorMessage = "La description de l'annonce est obligatoire")]
            [StringLength(2500, MinimumLength = 115, ErrorMessage = "Un Minimum de 115 caractères et Maximum de 2500 caractères est obligatoire.")]
            public string ad_description { get; set; }

            [Required]
            public int ad_status { get; set; }
            [Required]
            public int ad_is_published { get; set; }

            [Required]
            public string ad_name { get; set; }

            [Required(ErrorMessage = "Il faut un email associé a l'annonce.")]
            [EmailAddress(ErrorMessage="Le format de l'email est incorrect")]    
            public string ad_email { get; set; }

             [RegularExpression("^[0-9]{7,12}$", ErrorMessage = "Le format du telephone est incorret - Minimum 7, Maximum 11 caracteres numerique")]
            public string ad_phone { get; set; }
            [Required]
            public System.DateTime ad_date_created { get; set; }
            [Required]
            public System.DateTime ad_date_expired { get; set; }

            public int id_item_condition { get; set; }

            [Required(ErrorMessage = "le prix de l'article est obligatoire")]
            [Range(0,999999999,ErrorMessage="le prix de l'article doit ete entre 0 et 999999999")]
            public double ad_price { get; set; }
            [Required(ErrorMessage="If faut precisez le type de l'annonce")]
            public int ad_type { get; set; }
             [Required(ErrorMessage = "Votre commune est obligatoire")]            
            public int id_commune { get; set; }
            [Required(ErrorMessage = "Votre departement est obligatoire")]
            public int id_departement { get; set; }
            [Required]
            public int id_account { get; set; }
            [Required]
            public int UserId { get; set; }
             [Required(ErrorMessage = "La devise est obligatoire")]
            public int id_devise { get; set; }
        }
    }
}
