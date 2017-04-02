using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.Web.Areas.Admin.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace B1625MVC.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AccountController : Controller
    {
        public IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IUserService>();
            }
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string username)
        {
            EditUserViewModel viewData = null;
            var userData = await UserService.GetByNameAsync(username);
            if (userData != null)
            {
                viewData = new EditUserViewModel()
                {
                    Id = userData.Id,
                    UserName = userData.UserName,
                    Email = userData.Email,
                    Avatar = userData.Avatar,
                    Roles = userData.Roles
                };
                return View(viewData);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(HttpPostedFileBase avatarFile, EditUserViewModel userData)
        {
            if (ModelState.IsValid)
            {
                byte[] avatar = null;
                if (avatarFile != null && avatarFile.ContentLength > 0)
                {
                    byte[] buffer = new byte[avatarFile.ContentLength];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = avatarFile.InputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        avatar = ms.ToArray();
                    }
                }
                var old = await UserService.GetByIdAsync(userData.Id);
                EditUserData userDto = new EditUserData()
                {
                    Id = userData.Id,
                    UserName = userData.UserName,
                    Email = userData.Email,
                    Roles = userData.Roles,
                    Avatar = avatar,
                    NewPassword = userData.NewPassword
                };

                var result = await UserService.EditAsync(userDto);
                if (result.Succedeed)
                {
                    if(!string.IsNullOrEmpty(userData.NewPassword) && User.Identity.Name == old.UserName)
                    {
                        return RedirectToRoute(new { area = "", controller = "Account", action = "Logout" });
                    }
                    return RedirectToRoute(new { area = "Admin", controller = "Account", action = "List" });
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(userData);
        }

        public async Task<ActionResult> Details(string username)
        {
            UserInfo user = await UserService.GetByNameAsync(username);
            if (user != null)
            {
                return View(user);
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserViewModel userData, string[] roles)
        {
            if (ModelState.IsValid)
            {
                CreateUserData userDto = new CreateUserData()
                {
                    UserName = userData.UserName,
                    Email = userData.Email,
                    NewPassword = userData.Password,
                    Roles = roles ?? new string[] { "Users" }
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

        public ActionResult UsersTable(string usernameFilter, int page = 1)
        {
            //TODO: Add pages to show more users
            PageInfo pageInfo = new PageInfo(page, 20);

            IEnumerable<UserInfo> users;
            if (string.IsNullOrEmpty(usernameFilter))
            {
                users = UserService.GetUsers(pageInfo);
            }
            else
            {
                users = UserService.Find(ud => ud.UserName == usernameFilter, pageInfo);
            }
            ViewBag.UsernameFilter = usernameFilter;
            return PartialView("_UsersTable", users);
        }
    }
}