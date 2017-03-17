using B1625MVC.Model.Abstract;
using B1625MVC.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B1625MVC.Web.Controllers
{
    public class AccountController : Controller
    {
        private IB1625Repository _dataRepository;

        public AccountController(IB1625Repository repository) : base()
        {
            _dataRepository = repository;
        }

        // GET: Account
        public ActionResult UserProfile(string username, long? id)
        {
            var user = _dataRepository.Accounts.FirstOrDefault(ua => ua.Username == username || ua.UserId == id);

            if (user != null)
            {
                return View(user);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}