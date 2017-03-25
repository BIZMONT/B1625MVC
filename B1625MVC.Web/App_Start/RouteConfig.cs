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
                url: "login",
                defaults: new { controller = "Account", action = "Login" },
                namespaces: new string[] { "B1625MVC.Web.Controllers" }
            );

            routes.MapRoute(
                name: null,
                url: "logout",
                defaults: new { controller = "Account", action = "Logout" },
                namespaces: new string[] { "B1625MVC.Web.Controllers" }
            );

            routes.MapRoute(
                name: null,
                url: "registration",
                defaults: new { controller = "Account", action = "Registration" },
                namespaces: new string[] { "B1625MVC.Web.Controllers" }
            );

            routes.MapRoute(
                name: null,
                url: "post/{id}",
                defaults: new { controller = "Publication", action = "Index" }
            );

            routes.MapRoute(
                name: null,
                url: "account/{username}",
                defaults: new { controller = "Account", action = "UserProfile" },
                 namespaces: new string[] { "B1625MVC.Web.Controllers" }
            );

            routes.MapRoute(
                name: null,
                url: "{action}",
                defaults: new { controller = "Home", action = "Hot" },
                namespaces: new string[] { "B1625MVC.Web.Controllers" }
            );
        }
    }
}
