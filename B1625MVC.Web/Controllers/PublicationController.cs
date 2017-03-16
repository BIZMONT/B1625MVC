using System;
using System.Web.Mvc;

using System.Linq;

using B1625MVC.Model.Abstract;

namespace B1625MVC.Web.Controllers
{
    public class PublicationController : Controller
    {
        private IB1625Repository _dataRepository;

        public PublicationController(IB1625Repository repository) : base()
        {
            _dataRepository = repository;
        }

        public ActionResult Index(long id)
        {
            var publication = _dataRepository.Publications.FirstOrDefault(p => p.PublicationId == id);

            if (publication == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(publication);
            }
        }
    }
}