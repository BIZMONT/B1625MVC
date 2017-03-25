using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.Web.Areas.Admin.Models;
using B1625MVC.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var users = UserService.GetAll();
            return View(users);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string accountId)
        {
            EditUserViewModel viewData = null;
            var userData = await UserService.GetAsync(accountId);
            if(userData != null)
            {
                //TODO: Edit user
            }
            return View(viewData);
        }

        public async Task<ActionResult> Edit(EditUserViewModel userData)
        {
            if(ModelState.IsValid)
            {
                UserDataDto userDto = new UserDataDto()
                {
                    //TODO: Add data
                };
                var result = await UserService.UpdateAsync(userDto);
                if(result.Succedeed)
                {
                    return RedirectToRoute(new { area = "Admin", controller = "Account", action = "List" });
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(userData);
        }

        public async Task<ActionResult> Details(string accountId)
        {
            UserDataDto user = await UserService.GetAsync(accountId);
            return View(user);
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
                UserDataDto userDto = new UserDataDto()
                {
                    UserName = userData.UserName,
                    Email = userData.Email
                };
                var result = await UserService.CreateAsync(userDto, userData.Password, roles);
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
    }
}