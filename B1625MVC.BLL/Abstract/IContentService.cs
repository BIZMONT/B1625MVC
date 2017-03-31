using B1625MVC.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B1625MVC.BLL.Abstract
{
    public interface IContentService : IDisposable
    {
        IEnumerable<PublicationInfo> FindPublications(Func<PublicationInfo, bool> predicate);
        Task<OperationDetails> CreatePublication(CreatePublicationData publicationData);
        IEnumerable<PublicationInfo> GetAllPublications();
        PublicationInfo GetPublication(long publicationId);
        Task<OperationDetails> RatePublication(long publicationId, string name, RateAction up);
        IEnumerable<CommentInfo> GetPublicationComments(long publicationId);

        Task<OperationDetails> RateComment(long commentId, string name, RateAction up);
        CommentInfo GetComment(long commentId);
        IEnumerable<CommentInfo> FindComments(Func<CommentInfo, bool> predicate);
        Task<OperationDetails> AddComment(CommentInfo comment, long publicationId);
    }
}
