using B1625MVC.BLL.DTO;
using System.Collections.Generic;

namespace B1625MVC.Web.Models
{
    public class PublicationViewModel
    {
        public PublicationInfo Publication
        {
            get; private set;
        }

        public IEnumerable<CommentInfo> Comments
        {
            get; private set;
        }
        public PublicationViewModel(PublicationInfo publication, IEnumerable<CommentInfo> comments)
        {
            Publication = publication;
            Comments = comments;
        }
    }
}