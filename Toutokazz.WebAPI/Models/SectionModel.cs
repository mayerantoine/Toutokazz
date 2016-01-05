using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Toutokazz.WebAPI.Models
{
    public class SectionModel
    {
        public int id_section { get; set; }
        public string section_title { get; set; }
        public  List<CategoryModel>  categories {get; set;}
    }
}