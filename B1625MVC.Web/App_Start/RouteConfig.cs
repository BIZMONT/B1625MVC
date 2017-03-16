using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace B1625MVC.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "{action}",
                defaults: new { controller = "Home", action = "Hot" }
            );
            routes.MapRoute(
                name: null,
                url: "post/{id}",
                defaults: new { controller = "Publication", action = "Index" }
            );
        }
    }
}
