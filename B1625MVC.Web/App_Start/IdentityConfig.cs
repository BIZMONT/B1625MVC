using B1625MVC.BLL;
using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.Services;
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
            appBuilder.CreatePerOwinContext<IUserService>(()=> ServiceProvider.GetUserService("B1625Db"));
            appBuilder.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                LoginPath = new PathString("/login"),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });
        }
    }
}