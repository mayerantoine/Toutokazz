using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Toutokaz.WebUI.Areas.Admin.Models
{
    public class SectionViewModel
    {
        [Required]
        public string section_title { get; set; }
        [Required]
        public string section_description { get; set; }
    }
}