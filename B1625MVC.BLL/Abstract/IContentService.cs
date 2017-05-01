using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using B1625MVC.BLL.DTO;
using B1625MVC.BLL.DTO.ContentData.CommentData;
using B1625MVC.BLL.DTO.Enums;
using B1625MVC.BLL.DTO.ContentData.PublicationData;
using System.Linq.Expressions;

namespace B1625MVC.BLL.Abstract
{
    public interface IContentService : IDisposable
    {
        Task<OperationDetails> CreatePublication(CreatePublicationData publicationData);
        PublicationInfo GetPublication(long publicationId);
        IEnumerable<PublicationInfo> FindPublications(Expression<Func<PublicationInfo, bool>> predicate, PageInfo pageInfo);
        Task<IEnumerable<PublicationInfo>> GetPublicationsAsync(PageInfo pageInfo);
        Task<IEnumerable<PublicationInfo>> GetBestPublicationsAsync(PageInfo pageInfo);
        Task<IEnumerable<PublicationInfo>> GetHotPublicationsAsync(PageInfo pageInfo);
        Task<OperationDetails> RatePublication(long publicationId, string username, RateAction rateAction);
        OperationDetails DeletePublication(long publicationId);
        OperationDetails EditPublication(EditPublicationData publicationData);

        OperationDetails AddComment(CreateCommentData comment, long publicationId);
        CommentInfo GetComment(long commentId);
        IEnumerable<CommentInfo> GetPublicationComments(long publicationId);
        Task<OperationDetails> RateComment(long commentId, string username, RateAction rateAction);
        OperationDetails DeleteComment(long commentId);
        IEnumerable<CommentInfo> FindComments(Expression<Func<CommentInfo, bool>> predicate, PageInfo pageInfo);
        CommentInfo GetBestComment();

        IEnumerable<UserInfo> GetTopUsers(int count);
        PublicationInfo GetBestPublication();
    }
}