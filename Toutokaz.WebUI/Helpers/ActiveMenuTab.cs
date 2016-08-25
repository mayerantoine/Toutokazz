using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Toutokaz.WebUI.Helpers
{
    public static class ActiveMenuTab
    {
        public static string ActiveTab(this AjaxHelper helper, string controller, string action, string param)
        {
            string classValue = "";
            string currentParam = "";

            string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
            string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();

            if (param != null)
            {
                currentParam = helper.ViewContext.Controller.ValueProvider.GetValue("id_account_type").ToString();
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