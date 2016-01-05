using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toutokaz.Domain.Models;

namespace Toutokaz.WebUI.Models
{
    public class DeposerAnnonceModel
    {
        public tb_ads modelannonce { get; set; }
        public LoginViewModel login { get; set; }
        public tb_ad_vehicule vehicule { get; set; }
        public tb_ad_immobilier immobilier { get; set; }
        public tb_ad_chaussure chaussure { get; set; }
    }

    
}