using B1625MVC.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B1625MVC.BLL.Abstract
{
    public interface IContentService : IDisposable
    {
        IEnumerable<PublicationInfo> FindPublications(Func<PublicationInfo, bool> predicate);
        OperationDetails CreatePublication(CreatePublicationData publicationData);
        IEnumerable<PublicationInfo> GetAllPublications();
        IEnumerable<CommentInfo> GetPublicationComments(long publicationId);
        PublicationInfo GetPublication(long publicationId);
        Task<OperationDetails> RatePublication(long publicationId, string name, RateAction up);
        Task<OperationDetails> RateComment(long commentId, string name, RateAction up);
    }
}
