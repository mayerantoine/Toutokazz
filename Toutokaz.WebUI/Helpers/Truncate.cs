using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Toutokaz.WebUI.Helpers
{
    public  static class Truncate
    {
        public static IHtmlString TruncateDescription(this HtmlHelper helper , string input, int length)
        {
            if (input.Length > length)
            {
                return helper.Raw(input.Substring(0, length) + "...");
            }
            else
            {
                return  helper.Raw(input);
            }
        }
    }
}