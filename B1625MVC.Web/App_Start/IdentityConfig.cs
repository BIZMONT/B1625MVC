using B1625MVC.BLL;
using B1625MVC.Model.Identity;
using B1625MVC.Web.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Net;
using System.Net.Mail;
using System.Web.Configuration;

namespace B1625MVC.Web
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            //TODO: better initial configuration

            appBuilder.CreatePerOwinContext(() =>
            {
                var service = ServiceProvider.GetUserService("B1625Db");

                var emailServiceSettins = new EmailServiceSettings();
                if (emailServiceSettins.IsEmailServiceEnabled)
                {
                    var smtpClient = new SmtpClient();
                    smtpClient.Host = emailServiceSettins.Host;
                    smtpClient.Port = emailServiceSettins.Port;
                    smtpClient.EnableSsl = emailServiceSettins.SslEnabled;
                    smtpClient.Credentials = new NetworkCredential(emailServiceSettins.Login, emailServiceSettins.Password);

                    service.EmailService = new EmailService(smtpClient, emailServiceSettins.Login);
                }
                return service;
            });

            appBuilder.CreatePerOwinContext(() => ServiceProvider.GetContentService("B1625Db"));

            appBuilder.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                LoginPath = new PathString("/login"),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });
        }
    }
}