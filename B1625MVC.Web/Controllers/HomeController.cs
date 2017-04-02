using System.Web.Mvc;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using B1625MVC.BLL.Abstract;
using System;
using B1625MVC.BLL.DTO;
using System.Threading.Tasks;

namespace B1625MVC.Web.Controllers
{
    public class HomeController : Controller
    {
        private IContentService ContentService
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IContentService>();
            }
        }

        public async Task<ActionResult> Hot(int page = 1)
        {
            PageInfo pageInfo = new PageInfo(page, 50);
            var publications = await ContentService.GetPublicationsAsync(pageInfo);
            return View("Publications", publications);
        }

        public async Task<ActionResult> Fresh(int page = 1)
        {
            PageInfo pageInfo = new PageInfo(page, 50);
            var publications = await ContentService.GetPublicationsAsync(pageInfo);
            return View("Publications", publications);
        }

        public async Task<ActionResult> Best(int page = 1)
        {
            PageInfo pageInfo = new PageInfo(page, 50);
            var publications = await ContentService.GetBestPublicationsAsync(pageInfo);
            return View("Publications", publications);
        }
    }
}