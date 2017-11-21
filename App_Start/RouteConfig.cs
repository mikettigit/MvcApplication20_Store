using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApplication20
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Category",
                url: "Catalog/Category/{*query}",
                defaults: new { controller = "Catalog", action = "Category", query = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Item",
                url: "Catalog/Item/{*query}",
                defaults: new { controller = "Catalog", action = "Item", query = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}