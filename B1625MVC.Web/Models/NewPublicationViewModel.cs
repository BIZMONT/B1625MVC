using B1625MVC.BLL.DTO;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace B1625MVC.Web.Models
{
    public class NewPublicationViewModel
    {
        [Required]
        [MaxLength(32)]
        public string Title { get; set; }

        [Required]
        [UIHint("ContentTypeTemplate")]
        [Display(Name ="Publication type")]
        public ContentType ContentType { get; set; }

        public HttpFileCollectionBase ImageFile { get; set; }

        public string Text { get; set; }
    }
}