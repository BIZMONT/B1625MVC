using B1625MVC.BLL.Abstract;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace B1625MVC.Web.Areas.Admin.Controllers
{
    [Authorize(Roles ="Administrators")]
    public class CommentController : Controller
    {
        private IContentService ContentService
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IContentService>();
            }
        }

        public ActionResult Delete(long commentId)
        {
            var result = ContentService.DeleteComment(commentId);
            return Content(result.Message);
        }
    }
}