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
    public static class ActiveMenu
    {
        public static string ActivePage(this HtmlHelper helper, string controller, string action,string param)
        {
            string classValue = "";
            string currentParam = "";

            string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
            string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();

            if (param != null)
            {
                currentParam = helper.ViewContext.Controller.ValueProvider.GetValue("typeannonce").RawValue.ToString();
                if (currentController == controller && currentAction == action && currentParam == param)
                {
                    classValue = "active";
                }

            }
            else
            {

                if (currentController == controller && currentAction == action)
                {
                    classValue = "active";
                }
            }
          

            return classValue;
        }
    }
}