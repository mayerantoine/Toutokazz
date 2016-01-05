using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toutokaz.Domain.Models;


namespace Toutokaz.WebUI.Models
{
    public class HomePageModel
    {

        public IList<tb_ads> ImgNew { get; set; }
        public IList<tb_section> sectionList { get; set; }
    }
}