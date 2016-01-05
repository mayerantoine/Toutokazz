using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToutokazAdmin.WebUI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage="Vous devez avoir un email. un Email est obligatoire")]
        [EmailAddress(ErrorMessage="Format de  votre email est incorrect")]
        public string Email { get; set; }

        [Required(ErrorMessage="Vous devez taper votre mot de passe")]
        [DataType(DataType.Password,ErrorMessage="Format de mot de passe incorrect")]
        public string Password { get; set; }
    }
}