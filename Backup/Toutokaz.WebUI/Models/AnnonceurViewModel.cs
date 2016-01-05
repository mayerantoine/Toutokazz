using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toutokaz.Domain.Models;
using PagedList;
using PagedList.Mvc;

namespace Toutokaz.WebUI.Models
{
    public class AnnonceurViewModel
    {
        public string AnnonceurName { get; set; }
        public  tb_account annoneur { get; set; }
        public IPagedList<tb_ads> AdsList { get; set; }
    }
}