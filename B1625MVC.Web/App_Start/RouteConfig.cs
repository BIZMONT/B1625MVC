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
                url: "post/{id}",
                defaults: new { controller = "Publication", action = "Index" },
                namespaces: new string[] { "B1625MVC.Web.Controllers" }
            );

            routes.MapRoute(
                name: null,
                url: "publications/{action}",
                defaults: new { controller = "Home", action = "Hot" },
                namespaces: new string[] { "B1625MVC.Web.Controllers" }
            );

            routes.MapRoute(
                name: null,
                url: "ConfirmEmail/{userId}/{token}",
                defaults: new { controller = "Account", action = "ConfirmEmail" },
                 namespaces: new string[] { "B1625MVC.Web.Controllers" }
            );

            routes.MapRoute(
                name: null,
                url: "account/{username}",
                defaults: new { controller = "Account", action = "UserProfile" },
                 namespaces: new string[] { "B1625MVC.Web.Controllers" }
            );

            routes.MapRoute(
                name: null,
                url: "account/{username}/settings",
                defaults: new { controller = "Account", action = "Settings" },
                 namespaces: new string[] { "B1625MVC.Web.Controllers" }
            );


            routes.MapRoute(
                name: null,
                url: "{action}",
                defaults: new { controller = "Account" },
                namespaces: new string[] { "B1625MVC.Web.Controllers" }
            );

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Hot" },
                namespaces: new string[] { "B1625MVC.Web.Controllers" }
            );
        }
    }
}
