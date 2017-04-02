using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using B1625MVC.BLL.DTO;
using B1625MVC.BLL.DTO.ContentData.CommentData;
using B1625MVC.BLL.DTO.ContentData.PublicationData;
using B1625MVC.BLL.DTO.Enums;

namespace B1625MVC.BLL.Abstract
{
    public interface IContentService : IDisposable
    {
        Task<OperationDetails> CreatePublication(CreatePublicationData publicationData);
        PublicationInfo GetPublication(long publicationId);
        Task<IEnumerable<PublicationInfo>> GetPublicationsAsync(PageInfo pageInfo);
        Task<IEnumerable<PublicationInfo>> GetBestPublicationsAsync(PageInfo pageInfo);
        Task<OperationDetails> RatePublication(long publicationId, string username, RateAction rateAction);

        Task<OperationDetails> AddComment(CreateCommentData comment, long publicationId);
        CommentInfo GetComment(long commentId);
        IEnumerable<CommentInfo> GetPublicationComments(long publicationId);
        Task<OperationDetails> RateComment(long commentId, string username, RateAction rateAction);
    }
}