using B1625MVC.BLL.Abstract;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B1625MVC.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class PublicationController : Controller
    {
        private IContentService ContentService
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IContentService>();
            }
        }

        public ActionResult Delete(long publicationId)
        {
            var result = ContentService.DeletePublication(publicationId);
            return Content(result.Message);
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}