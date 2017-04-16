using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.Web.Infrastructure;
using B1625MVC.Web.Models;

namespace B1625MVC.Web.Controllers
{
    /// <summary>
    /// Class thai implements controller that work with users data
    /// </summary>
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

        /// <summary>
        /// Actiin that returns view with user profile data
        /// </summary>
        /// <param name="username">Name of user whose data will be displayed</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> UserProfile(string username)
        {
            var user = await UserService.GetByNameAsync(username); //find user by username in data source
            if (user != null) //if user exists
            {
                return View(user);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Action that returns view with current user settings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Settings()
        {
            UserInfo user = await UserService.GetByNameAsync(User.Identity.Name);
            var settingsModel = new ProfileSettingsViewModel()
            {
                Username = user.UserName,
                Email = user.Email,
                Gender = user.Gender
            };
            return View(settingsModel);
        }

        [HttpPost]
        public ActionResult Settings(ProfileSettingsViewModel model)
        {
            byte[] avatar = null;
            //TODO: Need image checker
            if (Request.Files.Count > 0)//if avatar vas uploaded
            {
                avatar = FileUploader.UploadFile(Request.Files[0]); //transform image to byte array
            }

            var task = UserService.GetByNameAsync(User.Identity.Name);
            var user = task.Result;

            var userData = new EditUserData()
            {
                Id = user.Id,
                Email = model.Email,
                NewPassword = model.NewPassword,
                Gender = model.Gender,
                Avatar = avatar
            };
            UserService.EditAsync(userData);
            return HttpNotFound();
        }

        /// <summary>
        /// Action that returns view with authentication form
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid) //if login data is correct
            {
                if (await LoginAsync(model.EmailOrUserName, model.Password, model.RememberMe)) //if user was authenticated successfully
                {
                    if (returnUrl == null) //check if return url is exists
                    {
                        return RedirectToRoute(new { controller = "Home", action = "Hot" }); //redirect to homepage
                    }
                    else
                    {
                        return Redirect(returnUrl); //retirect to return url
                    }
                }
                ModelState.AddModelError("", "Wrong username or password");//add error to model
            }
            return View(model); //return login form view
        }

        /// <summary>
        /// Action that sign out current user
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToRoute(new { controller = "Home", action = "Hot" });

        }

        /// <summary>
        /// Action that returns view with registration form
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid) //if registration data correct
            {
                var userData = new CreateUserData() //create new container for new user data
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    NewPassword = model.Password,
                    Roles = new string[] { "Users" }
                };

                var result = await UserService.CreateAsync(userData); //try to add new user

                if (result.Succedeed) //if user was added successfully
                {
                    var user = await UserService.GetByNameAsync(userData.UserName); //get new user data
                    var url = Url.Action("ConfirmEmail", "Account",null,Request.Url.Scheme);
                    await UserService.SendVerificationEmailAsync(user.Id, url); //send email confiramtion

                    return View("RegistrationCompleted");
                }
                ModelState.AddModelError("", result.Message); //add error to model
            }
            return View(model); //return registration form
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            UserInfo user = await UserService.GetByIdAsync(userId);
            if (user != null && await UserService.CheckEmailToken(user.Id, token))
            {
                ViewBag.Succeeded = true;
            }
            else
            {
                ViewBag.Succeeded = false;
            }

            return View();
        }

        /// <summary>
        /// Method for user authentication
        /// </summary>
        /// <param name="emailOrUsername"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns>Resturns authentication results</returns>
        private async Task<bool> LoginAsync(string emailOrUsername, string password, bool rememberMe)
        {
            var claim = await UserService.AuthenticateAsync(emailOrUsername, password); //get claims of user
            if (claim != null) // if claims created successfully
            {
                AuthenticationManager.SignOut(); //sign out if current user is already authenticated
                AuthenticationManager.SignIn(new AuthenticationProperties() //sign in with claims
                {
                    IsPersistent = rememberMe
                }, claim);
                return true;
            }
            return false;
        }
    }
}