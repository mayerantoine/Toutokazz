using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Toutokaz.WebUI.Models
{
    public class ProfileViewModel
    {
        public int id_account { get; set; }
        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        public string nom_entreprise { get; set; }

        public string telephone { get; set; }

        public string address { get; set; }

        public string alias { get; set;}
    }
}