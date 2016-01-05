using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toutokaz.WebUI.Models
{
    public class AccountViewModel
    {
        public int id_account;
        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int account_type { get; set; }

        public string nom_entreprise { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("password")]
        public string confirmPassword { get; set; }

        public string telephone { get; set; }

        public string address { get; set; }
    }

    public class RegisterExternalLoginModel
    {

        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string ExternalLoginID { get; set; }
    }
}