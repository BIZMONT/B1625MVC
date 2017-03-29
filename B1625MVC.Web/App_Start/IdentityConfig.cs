using B1625MVC.BLL;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace B1625MVC.Web
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.CreatePerOwinContext(() => ServiceProvider.GetUserService("B1625Db"));
            appBuilder.CreatePerOwinContext(() => ServiceProvider.GetContentService("B1625Db"));
            appBuilder.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                LoginPath = new PathString("/login"),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });
        }
    }
}