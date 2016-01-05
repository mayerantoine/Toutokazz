using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toutokaz.WebUI.Models
{
    public class ContacterViewModel
    {
        [Required]
        public int idad { get; set; }
        public string ad_title { get; set; }
        public string ad_code{ get; set; }
        [Required]
        [EmailAddress]
        public string Emailaccount { get; set; }
        [EmailAddress]
        [Required(ErrorMessage="Votre email est obligatoire")]
        public string Email { get; set; }
        public string telephone { get; set; }
        [Required(ErrorMessage="Le message est obligatoire")]
        public string message { get; set; }
    }
}