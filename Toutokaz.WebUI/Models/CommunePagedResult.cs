using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Toutokaz.WebUI.Models
{
    public class CommunePagedResult
    {
        public int Total { get; set; }
        public List<CommuneResult> Results { get; set; }
    }
}