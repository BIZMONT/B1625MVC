using System.Web.Mvc;
using System.Linq;

using B1625MVC.Model.Abstract;

namespace B1625MVC.Web.Controllers
{
    public class HomeController : Controller
    {
        private IB1625Repository _dataRepository;

        public HomeController(IB1625Repository repository) : base()
        {
            _dataRepository = repository;
        }

        public ActionResult Hot(int page = 1)
        {
            var publications = _dataRepository.Publications.Skip(20 * (page - 1)).Take(20);

            return View("Publications", publications);
        }

        public ActionResult Fresh(int page = 1)
        {
            var publications = _dataRepository.Publications.OrderBy(p=>p.PublicationDate).Skip(20 * (page - 1)).Take(20);

            return View("Publications", publications);
        }

        public ActionResult Best(int page = 1)
        {
            var publications = _dataRepository.Publications.OrderByDescending(p=>p.Rating).Skip(20 * (page - 1)).Take(20);

            return View("Publications", publications);
        }
    }
}