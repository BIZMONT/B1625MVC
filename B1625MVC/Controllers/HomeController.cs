using B1625DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B1625MVC.Controllers
{
    public class HomeController : Controller
    {
        private B1625DbRepository _dataRepository;
        public HomeController() : base()
        {
            _dataRepository = new B1625DbRepository();
        }

        public ActionResult Hot(int page = 1)
        {
            ViewBag.Title = "Hot";
            var publications = _dataRepository.GetHotPublications(page);
            return View("Publications", publications);
        }

        public ActionResult Fresh(int page = 1)
        {
            ViewBag.Title = "Fresh";
            var publications = _dataRepository.GetFreshPublications(page);

            return View("Publications", publications);
        }

        public ActionResult Best(int page = 1)
        {
            ViewBag.Title = "Best";
            var publications = _dataRepository.GetBestPublications(page);
            return View("Publications", publications);
        }
    }
}