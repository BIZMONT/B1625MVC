using B1625MVC.BLL;
using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace B1625MVC.Web.Controllers
{
    public class CommentController : Controller
    {
        public IContentService ContentService
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IContentService>();
            }
        }

        [Authorize]
        public async Task<string> RateUp(long commentId)
        {
            OperationDetails result = await ContentService.RateComment(commentId, User.Identity.Name, RateAction.Up);
            if (result.Succedeed)
            {
            }
            return ContentService.GetComment(commentId).Rating.ToString();
        }

        [Authorize]
        public async Task<string> RateDown(long commentId)
        {
            OperationDetails result = await ContentService.RateComment(commentId, User.Identity.Name, RateAction.Down);
            if (result.Succedeed)
            {
            }
            return ContentService.GetComment(commentId).Rating.ToString();
        }

        public ActionResult PublicationComments (long publicationId)
        {
            //var comments = ContentService.FindComments(c => c.PublicationId == publicationId);
            return PartialView("_Comments");
        }
    }
}