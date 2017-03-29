using System.Web.Mvc;

namespace B1625MVC.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: null,
                url: "Admin/Users/Details/{username}",
                defaults: new { controller = "Account", action = "Details" }
            );
            context.MapRoute(
                name: null,
                url: "Admin/Users/{action}",
                defaults: new { controller = "Account", action = "List" }
            );
            context.MapRoute(
                name: null,
                url: "Admin",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}