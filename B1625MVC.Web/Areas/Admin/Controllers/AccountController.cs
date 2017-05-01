using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.Web.Areas.Admin.Models;
using B1625MVC.Web.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace B1625MVC.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AccountController : Controller
    {
        /// <summary>
        /// Property for getting user service from owin context
        /// </summary>
        public IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IUserService>();
            }
        }

        /// <summary>
        /// Action that returns view with list of all registrated users
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string username)
        {
            EditUserViewModel viewData = null;
            if (username != null)
            {
                var userData = await UserService.GetByNameAsync(username);
                if (userData != null)
                {
                    viewData = new EditUserViewModel()
                    {
                        Id = userData.Id,
                        UserName = userData.UserName,
                        Email = userData.Email,
                        Avatar = userData.Avatar,
                        Gender = userData.Gender,
                        Roles = UserService.GetAllRoles().Select(r => new RoleCheckModel() { Name = r, Checked = userData.Roles.Contains(r) }).ToList()
                    };
                    ViewBag.Roles = UserService.GetAllRoles();
                    return View(viewData);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditUserViewModel userData)
        {
            if (ModelState.IsValid)
            {
                byte[] avatar = null;
                if (Request.Files.Count > 0 &&
                    (Request.Files[0].ContentType == System.Net.Mime.MediaTypeNames.Image.Gif ||
                    Request.Files[0].ContentType == System.Net.Mime.MediaTypeNames.Image.Jpeg))
                {
                    avatar = FileUploader.UploadFile(Request.Files[0]); //Convert uploaded file to byte array
                }

                UserInfo oldUserData = await UserService.GetByIdAsync(userData.Id);
                EditUserData userDto = new EditUserData()
                {
                    Id = userData.Id,
                    UserName = userData.UserName,
                    Email = userData.Email,
                    Roles = userData.Roles.Where(r => r.Checked).Select(r => r.Name),
                    Avatar = avatar,
                    Gender = userData.Gender,
                    NewPassword = userData.NewPassword
                };

                var result = await UserService.EditAsync(userDto);
                if (result.Succedeed)
                {
                    UserInfo user = await UserService.GetByNameAsync(userDto.UserName);
                    if (!user.EmailConfirmed)
                    {
                       await  UserService.SendVerificationEmailAsync(user.Id, Url.Action("ConfirmEmail", "Account", new { area = "" }));
                    }

                    if (!string.IsNullOrEmpty(userData.NewPassword) && User.Identity.Name == oldUserData.UserName)
                    {
                        return RedirectToRoute(new { area = "", controller = "Account", action = "Logout" });
                    }
                    return RedirectToRoute(new { area = "Admin", controller = "Account", action = "List" });
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(userData);
        }

        /// <summary>
        /// Action that returns view with detailed user information
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(string username)
        {
            //TODO: add user comments and posts to this information like in the main area account controller (or use that)
            UserInfo user = await UserService.GetByNameAsync(username); //get user info from source by username
            if (user != null) //if user with this username is exists
            {
                return View(user);
            }
            return HttpNotFound();
        }

        /// <summary>
        /// Action that returns view with form for creating new user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var model = new CreateUserViewModel();
            model.Roles = UserService.GetAllRoles().Select(r => new RoleCheckModel() { Name = r, Checked = false }).ToList(); //get all available roles from database
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserViewModel userData)
        {
            if (ModelState.IsValid)
            {
                CreateUserData userDto = new CreateUserData()
                {
                    UserName = userData.UserName,
                    Email = userData.Email,
                    NewPassword = userData.Password,
                    Roles = userData.Roles.Where(r => r.Checked).Select(r => r.Name)
                };
                var result = await UserService.CreateAsync(userDto);
                if (result.Succedeed)
                {
                    return RedirectToRoute(new { area = "Admin", controller = "Account", action = "List" });
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(userData);
        }

        public async Task<ActionResult> Delete(string accountId)
        {
            var result = await UserService.DeleteAsync(accountId);
            if (!result.Succedeed)
            {
                ModelState.AddModelError("", result.Message);
            }
            return RedirectToRoute(new { area = "Admin", controller = "Account", action = "List" });
        }

        /// <summary>
        /// Action that returns partial view with ine page of user table
        /// </summary>
        /// <param name="UsernameFilter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult UsersTable(string UsernameFilter, string RoleFilter, int page = 1)
        {
            //TODO: Add pages to show more users
            PageInfo pageInfo = new PageInfo(page, 20);

            IEnumerable<UserInfo> users = null;
            if (string.IsNullOrEmpty(UsernameFilter) && string.IsNullOrEmpty(RoleFilter)) //check if filter was used in current request
            {
                users = UserService.GetUsers(pageInfo); //get one page of all users from source
            }
            else
            {
                if (!string.IsNullOrEmpty(UsernameFilter))
                {
                    users = UserService.Find(ud => ud.UserName == UsernameFilter, pageInfo); //get users from source with filter
                }
                if(!string.IsNullOrEmpty(RoleFilter))
                {
                    if(users == null)
                    {
                        users = UserService.Find(ud => ud.Roles.Contains(RoleFilter), pageInfo);
                    }
                    else
                    {
                        users = users.Where(ud => ud.Roles.Contains(RoleFilter));
                    }
                }
            }
            ViewBag.UsernameFilter = UsernameFilter; //add filter data to view
            return PartialView("_UsersTable", users);
        }
    }
}