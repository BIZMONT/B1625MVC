using B1625MVC.BLL;
using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.BLL.DTO.ContentData.CommentData;
using B1625MVC.BLL.DTO.Enums;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace B1625MVC.Web.Controllers
{
    public class CommentController : Controller
    {
        /// <summary>
        /// Getter for content service to get access to database
        /// </summary>
        public IContentService ContentService
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IContentService>(); //get service from owin context
            }
        }

        /// <summary>
        /// Action that rate comment up by authorized user
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<int> RateUp(long commentId)
        {
            int currentRating = ContentService.GetComment(commentId).Rating; //get current rating of comment with commentId
            OperationDetails result = await ContentService.RateComment(commentId, User.Identity.Name, RateAction.Up); //rate comment by current user
            if (result.Succedeed)  //if comment was rated succesfully
            {
                currentRating++; //increment local value of rating for publication
            }
            return currentRating; //return comment rating
        }

        /// <summary>
        /// Action that rate comment down by authorized user
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<int> RateDown(long commentId)
        {
            int currentRating = ContentService.GetComment(commentId).Rating; //get current rating of comment with commentId
            OperationDetails result = await ContentService.RateComment(commentId, User.Identity.Name, RateAction.Down); //rate comment by current user
            if (result.Succedeed) //if comment was rated succesfully
            {
                currentRating--; //decrement local value of rating for publication
            }
            return currentRating; //return comment rating
        }

        /// <summary>
        /// Action that return partial view with list of comments for publication
        /// </summary>
        /// <param name="publicationId">Target publication</param>
        /// <returns></returns>
        public ActionResult PublicationComments(long publicationId)
        {
            var comments = ContentService.GetPublicationComments(publicationId); //get all comments for publication with publicationId
            if (comments != null) //if comments exists
            {
                return PartialView("_Comments", comments); //return view
            }
            return Content("Can`t load comments"); //return error message
        }

        [Authorize]
        /// <summary>
        /// Actiont that process post request for adding new comment to publication
        /// </summary>
        /// <param name="newComment">Comment content</param>
        /// <param name="publicationId">Commented publication id</param>
        /// <returns></returns>
        public ActionResult Add(string newComment, long publicationId)
        {
            if(!string.IsNullOrEmpty(newComment)) //if comment content is not empty
            {
                var result = ContentService.AddComment(new CreateCommentData()
                {
                    Author = User.Identity.Name,
                    Content = newComment
                }, publicationId); //try to add comment ot source

                if (!result.Succedeed)
                {
                    ModelState.AddModelError("", "Can`t add comment: " + result.Message);
                }
            }
            return PublicationComments(publicationId); //return result of another action
        }

        public ActionResult UserComments(string username, int page = 1)
        {
            PageInfo pageInfo = new PageInfo(page, 50); //Creating object that holds informations for pager
            IEnumerable<CommentInfo> comments = ContentService.FindComments(c => c.Author == username, pageInfo); //getting from source publications by user
            ViewBag.PageInfo = pageInfo;
            if(comments == null || comments.Count() == 0)
            {
                return Content("No content");
            }
            return PartialView("_Comments", comments);
        }

        [HttpGet]
        public ActionResult BestComment()
        {
            CommentInfo comment = ContentService.GetBestComment();
            if(comment !=null)
            {
                return PartialView("_SimpleCommentPartial", comment);
            }
            return Content("Not found");
        }
    }
}