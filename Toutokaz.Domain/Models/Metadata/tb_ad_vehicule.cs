using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toutokaz.Domain.Models
{
    [MetadataType(typeof(tb_ad_vehicule_metadata))]
    public partial class tb_ad_vehicule
    {
        internal class tb_ad_vehicule_metadata
        {
        
            [Required]
            int id_ad_vehicule { get; set; }
            [Required]
            int id_ad { get; set; }

            int annee { get; set; }
            int mileage { get; set; }
            int id_transmission { get; set; }
            int id_carburant { get; set; }
            string marque { get; set; }
            string modele { get; set; }
        }
    }
}
