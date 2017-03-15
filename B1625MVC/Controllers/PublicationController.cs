using B1625DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B1625MVC.Controllers
{
    public class PublicationController : Controller
    {
        private B1625DbRepository _dataRepository;

        public PublicationController() : base()
        {
            _dataRepository = new B1625DbRepository();
        }

        public ActionResult Index(long id)
        {
            var publication = _dataRepository.GetPublication(id);
            return View(publication);
        }

        public string RateUpPost(long id)
        {
            throw new NotImplementedException();
        }

        public string RateDownPost(long id)
        {
            throw new NotImplementedException();
        }

        public string RateUpComment(long id)
        {
            throw new NotImplementedException();
        }

        public string RateDownComent(long id)
        {
            throw new NotImplementedException();
        }
    }
}