using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToutokazAdmin.WebUI.Models
{
    public class AccountViewModel
    {
        [Required]
        public string Nom { get; set; }
   
        [Required]
        public string Prenom { get; set; }
       
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string nom_entreprise { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("password")]
        public string confirmPassword { get; set; }
    }
}