using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using B1625MVC.Web.Models;
using B1625MVC.BLL.Abstract;
using System;
using B1625MVC.BLL;
using B1625MVC.BLL.DTO;

namespace B1625MVC.Web.Controllers
{
    public class PublicationController : Controller
    {
        public IContentService ContentService
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IContentService>();
            }
        }

        public ActionResult Index(long id)
        {
            var publication = ContentService.GetPublication(id);
            if (publication == null)
            {
                return HttpNotFound();
            }
            else
            {
                var comments = ContentService.GetPublicationComments(publication.Id);
                var publicationModel = new PublicationViewModel(publication, comments);
                return View(publicationModel);
            }
        }

        [Authorize]
        public async Task<string> RateUp(long publicationId)
        {
            OperationDetails result = await ContentService.RatePublication(publicationId, User.Identity.Name, RateAction.Up);
            if (result.Succedeed)
            {
            }
            return ContentService.GetPublication(publicationId).Rating.ToString();
        }

        [Authorize]
        public async Task<string> RateDown(long publicationId)
        {
            OperationDetails result = await ContentService.RatePublication(publicationId, User.Identity.Name, RateAction.Down);
            if (result.Succedeed)
            {
            }
            return ContentService.GetPublication(publicationId).Rating.ToString();
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