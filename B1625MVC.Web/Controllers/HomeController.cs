using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using System.Threading.Tasks;

namespace B1625MVC.Web.Controllers
{
    public class HomeController : Controller
    {
        private const int publicationsPerPage = 50; //TODO: add paging as user preferense

        /// <summary>
        /// Getter for content service to get access to database
        /// </summary>
        private IContentService ContentService
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IContentService>(); 
            }
        }

        public async Task<ActionResult> Hot(int page = 1)
        {
            PageInfo pageInfo = new PageInfo(page, publicationsPerPage);
            var publications = await ContentService.GetHotPublicationsAsync(pageInfo);

            ViewBag.PageInfo = pageInfo;
            return View("Publications", publications);
        }

        public async Task<ActionResult> Fresh(int page = 1)
        {
            PageInfo pageInfo = new PageInfo(page, publicationsPerPage);
            var publications = await ContentService.GetPublicationsAsync(pageInfo);

            ViewBag.PageInfo = pageInfo;
            return View("Publications", publications);
        }

        public async Task<ActionResult> Best(int page = 1)
        {
            PageInfo pageInfo = new PageInfo(page, publicationsPerPage);
            var publications = await ContentService.GetBestPublicationsAsync(pageInfo);

            ViewBag.PageInfo = pageInfo;
            return View("Publications", publications);
        }
    }
}