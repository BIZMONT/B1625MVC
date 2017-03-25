using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using B1625MVC.Web.Models;
using B1625MVC.BLL.Abstract;
using System;

namespace B1625MVC.Web.Controllers
{
    public class PublicationController : Controller
    {
        public IContentService DataRepository
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IContentService>();
            }
        }

        public ActionResult Index(long id)
        {
            /*Publication publication = DataRepository.Publications.Get(id);

            if (publication == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(publication);
            }*/
            throw new NotImplementedException();
        }

        [Authorize]
        public async Task<string> RateUp(long publicationId)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public async Task <string> RateDown(long publicationId)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public ActionResult Add()
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public Task<ActionResult> Add(PublicationViewModel model)
        {
            //TODO: Add publication to database
            throw new NotImplementedException();
        }
    }
}