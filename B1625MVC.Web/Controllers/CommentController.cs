using B1625MVC.BLL;
using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.BLL.DTO.ContentData.CommentData;
using B1625MVC.BLL.DTO.Enums;
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
        public async Task<int> RateUp(long commentId)
        {
            int currentRating = ContentService.GetComment(commentId).Rating;
            OperationDetails result = await ContentService.RateComment(commentId, User.Identity.Name, RateAction.Up);
            if (result.Succedeed)
            {
                currentRating++;
            }
            return currentRating;
        }

        [Authorize]
        public async Task<int> RateDown(long commentId)
        {
            int currentRating = ContentService.GetComment(commentId).Rating;
            OperationDetails result = await ContentService.RateComment(commentId, User.Identity.Name, RateAction.Down);
            if (result.Succedeed)
            {
                currentRating--;
            }
            return currentRating;
        }

        public ActionResult PublicationComments(long publicationId)
        {
            var comments = ContentService.GetPublicationComments(publicationId);
            if (comments != null)
            {
                return PartialView("_Comments", comments);
            }
            return Content("Can`t load comments");
        }

        public ActionResult Add(string newComment, long publicationId)
        {
            var result = ContentService.AddComment(new CreateCommentData()
            {
                Author = User.Identity.Name,
                Content = newComment
            }, publicationId);

            if(!result.Succedeed)
            {
                ModelState.AddModelError("", "Can`t add comment: " + result.Message);
            }
            return PublicationComments(publicationId);
        }
    }
}