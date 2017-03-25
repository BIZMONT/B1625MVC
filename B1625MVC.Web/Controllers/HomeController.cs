using System.Web.Mvc;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using B1625MVC.BLL.Abstract;
using System;

namespace B1625MVC.Web.Controllers
{
    public class HomeController : Controller
    {
        private IContentService DataRepository
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IContentService>();
            }
        }

        public ActionResult Hot(int page = 1)
        {
            /*var publications = DataRepository.Publications.Skip(20 * (page - 1)).Take(20);

            return View("Publications", publications);
            */
            return HttpNotFound();
        }

        public ActionResult Fresh(int page = 1)
        {
            /*var publications = DataRepository.Publications.OrderBy(p => p.PublicationDate).Skip(20 * (page - 1)).Take(20);

            return View("Publications", publications);
            */
            return HttpNotFound();
        }

        public ActionResult Best(int page = 1)
        {
            /*var publications = DataRepository.Publications.OrderByDescending(p => p.Rating).Skip(20 * (page - 1)).Take(20);

            return View("Publications", publications);
            */
            return HttpNotFound();
        }
    }
}