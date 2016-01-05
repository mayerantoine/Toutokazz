using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toutokaz.Domain.Models;
using PagedList;
using PagedList.Mvc;

namespace Toutokaz.WebUI.Models
{
    public class CategoryPageModel
    {
        public string CategoryName { get; set; }
        public tb_category category { get; set; }
        public IList<tb_ads> AdsPremium{ get; set; }
        public IPagedList<tb_ads> AdsList { get; set; }
    }
}