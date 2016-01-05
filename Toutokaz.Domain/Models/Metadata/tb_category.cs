using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toutokaz.Domain.Models
{
     [MetadataType(typeof(tb_category_metadata))]
     public partial  class tb_category
    {
         internal class tb_category_metadata
         {
             public int id_category { get; set; }
             [Required]
             public int id_section { get; set; }
             [Required]
             public string category_title { get; set; }
             [Required]
             public string category_desctiption { get; set; }
         }
    }
}
