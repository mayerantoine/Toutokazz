using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Toutokaz.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
            "homepage",
            "quisommesnous",
            new { controller = "Home", action = "quisommesnous" },
            new[] { "Toutokaz.WebUI.Controllers" }
            );

            routes.MapRoute(
          "conditions",
          "conditionsdutilisation",
          new { controller = "Home", action = "conditionsdutilisation" },
          new[] { "Toutokaz.WebUI.Controllers" }
          );

            routes.MapRoute(
            "faq",
            "faq",
            new { controller = "Home", action = "faq" },
            new[] { "Toutokaz.WebUI.Controllers" }
            );

            routes.MapRoute(
            "contacteznous",
            "contactez-nous",
            new { controller = "Contact", action = "contact" },
            new[] { "Toutokaz.WebUI.Controllers" }
            );

            routes.MapRoute(
            "annonceur",
            "annonces/annonceur/{id}/{annonceurname}",
            new { controller = "annonces", action = "annonceur", id = UrlParameter.Optional, annonceurname = UrlParameter.Optional },
             new { id = @"\d+" },
               new[] { "Toutokaz.WebUI.Controllers" }
            );


            routes.MapRoute(
            "category",
            "annonces/{id_section}/{section_title}/{id_category}/{category_title}",
            new { controller = "annonces", action = "allannonce", id_section = UrlParameter.Optional, section_title= UrlParameter.Optional, id_category = UrlParameter.Optional, category_title = UrlParameter.Optional },
             new { id_category = @"\d+", id_section = @"\d+" },
               new[] { "Toutokaz.WebUI.Controllers" }
            );

            routes.MapRoute(
            "villes",
            "annonces/{ad_city}/{commune_title}",
            new { controller = "annonces", action = "allannonce", ad_city = UrlParameter.Optional, commune_title = UrlParameter.Optional },
             new { ad_city = @"\d+"},
               new[] { "Toutokaz.WebUI.Controllers" }
            );
            
            routes.MapRoute(
               "details",
              "annonces/details/{id}/{ad_title}",
              new  { controller = "Annonces", action = "Details", id= UrlParameter.Optional,  ad_title = UrlParameter.Optional },
              new { id = @"\d+" },
              new[] { "Toutokaz.WebUI.Controllers" }
            );

            routes.MapRoute(
           "compteparticulier",
           "compte/creer-compte",
           new { controller = "Account", action = "Create" },
           new[] { "Toutokaz.WebUI.Controllers" }
           );

            routes.MapRoute(
          "comptepro",
          "compte/creer-compte-professionel",
          new { controller = "Account", action = "CreatePro" },
          new[] { "Toutokaz.WebUI.Controllers" }
          );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Toutokaz.WebUI.Controllers" }
            );
        }
    }
}