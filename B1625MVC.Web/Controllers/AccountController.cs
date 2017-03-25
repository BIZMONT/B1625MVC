using B1625MVC.BLL.Abstract;
using B1625MVC.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace B1625MVC.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IUserService>();
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /*[HttpGet]
        public async Task<ActionResult> UserProfile(string username)
        {
            //TODO: Get user data from repository if exist
            throw new NotImplementedException();
        }*/

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var claim = await UserService.AuthenticateAsync(model.EmailOrUserName, model.Password);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Wrong email or password");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberMe
                    }, claim);
                    if(returnUrl == null)
                    {
                        return RedirectToRoute(new { controller = "Home" });
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
            }
            return View(model);
        }
        
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToRoute(new { controller = "Home" });

        }

        [AllowAnonymous]
        public ActionResult Registration()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Registration(RegistrationViewModel model)
        {
            if(ModelState.IsValid)
            {
                return Redirect("homepage");
            }
            else
            {
                return View(model);
            }
        }
    }
}