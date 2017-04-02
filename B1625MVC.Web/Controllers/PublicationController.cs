using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using B1625MVC.Web.Models;
using B1625MVC.BLL.Abstract;
using B1625MVC.BLL;
using B1625MVC.BLL.DTO;
using System.IO;
using System.Text;
using B1625MVC.BLL.DTO.Enums;

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
        public async Task<int> RateUp(long publicationId)
        {
            int currentRating = ContentService.GetPublication(publicationId).Rating;
            OperationDetails result = await ContentService.RatePublication(publicationId, User.Identity.Name, RateAction.Up);
            if (result.Succedeed)
            {
                currentRating++;
            }
            return currentRating;
        }

        [Authorize]
        public async Task<int> RateDown(long publicationId)
        {
            int currentRating = ContentService.GetPublication(publicationId).Rating;
            OperationDetails result = await ContentService.RatePublication(publicationId, User.Identity.Name, RateAction.Down);
            if (result.Succedeed)
            {
                currentRating--;
            }
            return currentRating;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Add(NewPublicationViewModel model)
        {
            byte[] content = null;
            if (model.ContentType == ContentType.Image)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    byte[] buffer = new byte[file.ContentLength];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = file.InputStream.Read(buffer, 0, file.ContentLength)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        content = ms.ToArray();
                    }
                }
            }
            else
            {
                content = Encoding.Default.GetBytes(model.Text);
            }

            var publication = new CreatePublicationData()
            {
                Author = User.Identity.Name,
                Content = content,
                ContentType = model.ContentType,
                Title = model.Title
            };
            var result = await ContentService.CreatePublication(publication);
            if (result.Succedeed)
            {
                return RedirectToRoute(new { controller = "Home", action = "Hot" });
            }

            ModelState.AddModelError("", "Cant`create publication: " + result.Message);
            return View();
        }
    }
}