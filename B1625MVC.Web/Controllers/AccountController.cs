using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> UserProfile(string username)
        {
            var user = await UserService.GetByNameAsync(username);
            if(user != null)
            {
                return View(user);
            }
            else
            {
                return HttpNotFound();
            }
        }


        [HttpGet]
        public ActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Settings(ProfileSettingsViewModel model)
        {
            var userData = new EditUserData()
            {
                Email = model.Email,
                NewPassword = model.NewPassword, Gender = model.Gender
            };
            UserService.EditAsync(userData);
            return HttpNotFound();
        }

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
                if (await LoginAsync(model.EmailOrUserName, model.Password, model.RememberMe))
                {
                    if (returnUrl == null)
                    {
                        return RedirectToRoute(new { controller = "Home", action = "Hot" });
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
                ModelState.AddModelError("", "Wrong username or password");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToRoute(new { controller = "Home", action = "Hot" });

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userData = new CreateUserData()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    NewPassword = model.Password,
                    Roles = new string[] { "Users" }
                };

                var result = await UserService.CreateAsync(userData);
                if (result.Succedeed)
                {
                    await LoginAsync(model.Email, model.Password, false);
                    return RedirectToRoute(new { controller = "Home", action = "Hot" });
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        private async Task<bool> LoginAsync(string emailOrUsername, string password, bool rememberMe)
        {
            var claim = await UserService.AuthenticateAsync(emailOrUsername, password);
            if (claim == null)
            {
                return false;
            }
            else
            {
                AuthenticationManager.SignOut();
                AuthenticationManager.SignIn(new AuthenticationProperties()
                {
                    IsPersistent = rememberMe
                }, claim);
                return true;
            }
        }
    }
}