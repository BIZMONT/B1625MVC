using B1625MVC.Web.Areas.Admin.Models;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Linq;
using B1625MVC.Web.Infrastructure;

namespace B1625MVC.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class HomeController : Controller
    {


        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditSmtpService()
        {
            var emailServiceSettings = new EmailServiceSettings();

            var smtpModel = new SmtpSettingsViewModel()
            {
                Enabled = emailServiceSettings.IsEmailServiceEnabled,
                Host = emailServiceSettings.Host,
                Port = emailServiceSettings.Port,
                Ssl = emailServiceSettings.SslEnabled,
                Login = emailServiceSettings.Login,
            };

            return PartialView("_SmtpSettings", smtpModel);
        }

        [HttpPost]
        public ActionResult EditSmtpService(SmtpSettingsViewModel model)
        {
            var emailServiceSettings = new EmailServiceSettings();
            if (ModelState.IsValid)
            {
                emailServiceSettings.IsEmailServiceEnabled = model.Enabled;
                emailServiceSettings.Host = model.Host;
                emailServiceSettings.Port = model.Port.Value;
                emailServiceSettings.SslEnabled = model.Ssl;
                emailServiceSettings.Login = model.Login;
                emailServiceSettings.Password = model.Password;
            }
            return PartialView("_SmtpSettings", model);
        }
    }
}