using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toutokaz.Domain.Models
{
     [MetadataType(typeof(tb_section_metadata))]
    public partial class tb_section
    {
        internal class tb_section_metadata
        {
            public int id_section { get; set; }
            [Required]
            public string section_title { get; set; }
            [Required]
            public string section_description { get; set; }
        }
    }
}
