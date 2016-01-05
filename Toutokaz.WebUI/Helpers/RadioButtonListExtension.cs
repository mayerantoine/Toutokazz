using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;

namespace Toutokaz.WebUI.Helpers
{
    public static class RadioButtonListExtension
    {

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(
    this HtmlHelper<TModel> htmlHelper,Expression<Func<TModel, TProperty>> expression,
    IEnumerable<SelectListItem> listOfValues,
    string cssClass = "",
    bool includeLabel = true,
    string selectedItemValue = "")
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();

            if (listOfValues != null)
            {
                // Create a radio button for each item in the list 
                foreach (SelectListItem item in listOfValues)
                {
                    // Generate an id to be given to the radio button field 
                    var id = string.Format("{0}_{1}", metaData.PropertyName, item.Value);
                    object htmlAttrib = new { id = id };

                    if (selectedItemValue == item.Value) htmlAttrib = new { id = id, @checked = "checked" };

                    // Create and populate a radio button using the existing html helpers 
                    var label = htmlHelper.Label(id, HttpUtility.HtmlDecode(item.Text));
                    var radio = htmlHelper.RadioButtonFor(expression, item.Value, htmlAttrib).ToHtmlString();

                    if (includeLabel == false)
                        label = null;

                    // Create the html string that will be returned to the client 
                    // e.g. <input data-val="true" data-val-required="You must select an 
                    // option" id="TestRadio_1" name="TestRadio" type="radio" value="1" />
                    // <label for="TestRadio_1">Line1</label> 
                    sb.AppendFormat("<div class=\"" + cssClass + "\">{0}{1}</div>", radio, label);

                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}