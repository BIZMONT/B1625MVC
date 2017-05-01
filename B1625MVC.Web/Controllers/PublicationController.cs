using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

using B1625MVC.BLL.DTO.Enums;
using B1625MVC.Web.Infrastructure;
using B1625MVC.BLL;
using B1625MVC.BLL.DTO;
using B1625MVC.Web.Models;
using B1625MVC.BLL.Abstract;
using System.Linq;

namespace B1625MVC.Web.Controllers
{
    public class PublicationController : Controller
    {
        /// <summary>
        /// Getter for content service to get access to database
        /// </summary>
        public IContentService ContentService
        {
            get
            {
                return HttpContext.GetOwinContext().Get<IContentService>();
            }
        }

        /// <summary>
        /// Method to get publication data view
        /// </summary>
        /// <param name="id">Publication id</param>
        /// <returns>Returns view with publication and it comments</returns>
        public ActionResult Index(long id)
        {
            var publication = ContentService.GetPublication(id); //get publication from source by publication id
            if (publication == null) //if publication with this id is no exists
            {
                return HttpNotFound();
            }
            else
            {
                var comments = ContentService.GetPublicationComments(publication.Id); //getting all comments for current publication
                var publicationModel = new PublicationViewModel(publication, comments); //creating view model with publication data and publication comments
                return View(publicationModel);
            }
        }

        /// <summary>
        /// Method for rate publication up by authorized user
        /// </summary>
        /// <param name="publicationId">Publication id that was rated</param>
        /// <returns>Returns current publlication rating after this action</returns>
        public async Task<int> RateUp(long publicationId)
        {
            int currentRating = ContentService.GetPublication(publicationId).Rating; //getting current rating of publication
            OperationDetails result = await ContentService.RatePublication(publicationId, User.Identity.Name, RateAction.Up); //rate publication by current user
            if (result.Succedeed) //if publication was rated succesfully
            {
                currentRating++; //increment local value of rating for publication
            }
            return currentRating;
        }

        /// <summary>
        /// Action that rate publication down by authorized user
        /// </summary>
        /// <param name="publicationId">Publication id that was rated</param>
        /// <returns>Returns current publlication rating after this action</returns>
        public async Task<int> RateDown(long publicationId)
        {
            int currentRating = ContentService.GetPublication(publicationId).Rating; //getting current rating of publication
            OperationDetails result = await ContentService.RatePublication(publicationId, User.Identity.Name, RateAction.Down); //rate publication by current user
            if (result.Succedeed) //if publication was rated succesfully
            {
                currentRating--; //decrement local value of rating for publication
            }
            return currentRating;
        }

        /// <summary>
        /// Method for getting view with form for add new publication
        /// </summary>
        /// <returns>Returns view with form</returns>
        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        
        /// <summary>
        /// Method for post request that adds new publication to database
        /// </summary>
        /// <param name="model">Model with data for new publication</param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> Add(NewPublicationViewModel model)
        {
            byte[] content = null; //array that will contain bytes of content
            if (model.ContentType == ContentType.Image)
            {
                //TODO: Bad code. Need holder for image types names
                if (Request.Files.Count > 0 && (Request.Files[0].ContentType == System.Net.Mime.MediaTypeNames.Image.Gif || Request.Files[0].ContentType == System.Net.Mime.MediaTypeNames.Image.Jpeg))
                {
                    content = FileUploader.UploadFile(Request.Files[0]); //Convert uploaded file to byte array
                }
                else //if file is empty or has unsupported type
                {
                    ModelState.AddModelError("ImageFile", "For image post you must upload image file"); //Add validation error to model
                    return View(model);
                }
            }
            else //if puplication has text content
            {
                content = Encoding.Default.GetBytes(model.Text); //convert text content to byte array
            }

            var publication = new CreatePublicationData() //create new publication object that contains main data to add to source
            {
                Author = User.Identity.Name,
                Content = content,
                ContentType = model.ContentType,
                Title = model.Title
            };
            var result = await ContentService.CreatePublication(publication); //trying to add publication data to source
            if (result.Succedeed) //if data was successfully added
            {
                return RedirectToRoute(new { controller = "Home", action = "Hot" });
            }

            ModelState.AddModelError("", "Cant`create publication: " + result.Message); //add validation error to model
            return View();
        }

        /// <summary>
        /// Partial method to get concrete user publications
        /// </summary>
        /// <param name="username"></param>
        /// <param name="page"></param>
        /// <returns>Returns partial view with list of user publications</returns>
        public ActionResult UserPublications(string username, int page = 1)
        {
            PageInfo pageInfo = new PageInfo(page, 50); //Creating object that holds informations for pager
            IEnumerable<PublicationInfo> publications = ContentService.FindPublications(p => p.Author == username, pageInfo); //getting from source publications by user
            ViewBag.PageInfo = pageInfo;
            if (publications == null || publications.Count() == 0)
            {
                return Content("No content");
            }
            return PartialView("_Publications", publications);
        }

        public ActionResult BestPublication()
        {
            PublicationInfo publication = ContentService.GetBestPublication();
            if(publication != null)
            {
                return PartialView("_SimplePublicationPartial", publication);
            }
            return Content("NotFound");
        }
    }
}