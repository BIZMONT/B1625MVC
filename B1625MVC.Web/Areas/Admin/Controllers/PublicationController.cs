using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.BLL.DTO.ContentData.PublicationData;
using B1625MVC.BLL.DTO.Enums;
using B1625MVC.Web.Areas.Admin.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [HttpGet]
        public ActionResult Edit(long publicationId)
        {
            PublicationInfo publication = ContentService.GetPublication(publicationId);
            return PartialView("_EditPublicationPartial", publication);
        }

        [HttpPost]
        public ActionResult Edit(EditPublicationViewModel publicationModel)
        {
            EditPublicationData editData = new EditPublicationData()
            {
                Id = publicationModel.Id,
                Title = publicationModel.Title,
                Content = Encoding.Default.GetBytes(publicationModel.Content)
            };

            var result = ContentService.EditPublication(editData);

            var fullPublication = ContentService.GetPublication(editData.Id);
            if(result.Succedeed)
            {
                return PartialView("_PublicationPartial", fullPublication);
            }
            ModelState.AddModelError("", result.Message);
            fullPublication.Title = editData.Title;
            fullPublication.Content = editData.Content;
            return PartialView("_EditPublicationPartial", fullPublication);
        }
    }
}