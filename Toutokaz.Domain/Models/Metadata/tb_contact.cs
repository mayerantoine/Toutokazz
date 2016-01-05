using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Toutokaz.Domain.Models
{
    [MetadataType(typeof(tb_contact_metadata))]
     public partial class tb_contact
    {
            internal class tb_contact_metadata
            {
                public int id_contact { get; set; }
                [Required(ErrorMessage="Votre Email est obligatoire")]
                [EmailAddress(ErrorMessage = "Le format de l'email est incorrect")]
                public string email { get; set; }
                [Required(ErrorMessage="Votre nom est obligatoire")]
                public string nom { get; set; }
                [Required(ErrorMessage="Votre Prenom est obligatoire")]
                public string prenom { get; set; }
                [Required(ErrorMessage="Le sujet du message est obligatoire")]
                public string sujet { get; set; }
                [Required(ErrorMessage="Le message est obligatoire")]
                [StringLength(200,ErrorMessage="Le nombre maximum de carateres est de 200")]
                
                public string message { get; set; }
                [RegularExpression("^\\D*(?:\\d\\D*){7,}$", ErrorMessage = "Le format du telephone est incorret")]
                public string telephone { get; set; }
            }
    }
}
