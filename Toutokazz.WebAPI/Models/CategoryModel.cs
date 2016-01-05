using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Toutokazz.WebAPI.Models
{
    public class CategoryModel
    {
        public int id_category { get; set; }
        public string category_title { get; set; }
        public int? category_order { get; set; }

    }

}