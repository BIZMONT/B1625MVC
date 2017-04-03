using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.BLL.DTO.Enums;
using B1625MVC.Model.Abstract;
using B1625MVC.Model.Entities;
using System.Data.Entity;
using System.Linq.Expressions;
using B1625MVC.BLL.DTO.ContentData.CommentData;

namespace B1625MVC.BLL.Services
{
    public class ContentService : IContentService
    {
        IRepository repo;

        public ContentService(IRepository repository)
        {
            repo = repository;
        }

        #region PublicationMethods
        public async Task<OperationDetails> CreatePublication(CreatePublicationData publicationData)
        {
            UserProfile author = repo.Profiles.Find(p => p.User.UserName == publicationData.Author).FirstOrDefault();

            if (author != null)
            {
                var newPublication = new Publication()
                {
                    Title = publicationData.Title,
                    Content = publicationData.Content,
                    PublicationDate = DateTime.Now,
                    ContentType = (Model.Enums.ContentType)Enum.Parse(typeof(Model.Enums.ContentType), Enum.GetName(typeof(ContentType), publicationData.ContentType)),
                    AuthorId = author.AccountId
                };

                repo.Publications.Create(newPublication);
                await repo.SaveChangesAsync();
                return new OperationDetails(true, "New publication was created successfully");
            }
            return new OperationDetails(false, "Can`t find author in database");
        }

        public async Task<IEnumerable<PublicationInfo>> FindPublication(Expression<Func<PublicationInfo, bool>> predicate, PageInfo pageInfo)
        {
            var allPublications = repo.Publications.Get().Select(p => new PublicationInfo()
            {
                Id = p.PublicationId,
                Author = p.Author.User.UserName,
                AuthorAvatar = p.Author.Avatar,
                CommentsCount = p.Comments.Count,
                Content = p.Content,
                ContentType = (ContentType)p.ContentType,
                PublicationDate = p.PublicationDate,
                Title = p.Title,
                Rating = p.Rating
            });
            return await allPublications.Where(predicate).OrderBy(p=>p.PublicationDate).Skip((pageInfo.CurrentPage - 1) * pageInfo.ElementsPerPage).Take(pageInfo.ElementsPerPage).ToListAsync();
        }

        public async Task<IEnumerable<PublicationInfo>> GetPublicationsAsync(PageInfo pageInfo)
        {
            var allPublications = repo.Publications.Get().Select(p => new PublicationInfo()
            {
                Id = p.PublicationId,
                Author = p.Author.User.UserName,
                AuthorAvatar = p.Author.Avatar,
                CommentsCount = p.Comments.Count,
                Content = p.Content,
                ContentType = (ContentType)p.ContentType,
                PublicationDate = p.PublicationDate,
                Title = p.Title,
                Rating = p.Rating
            });
            return await allPublications.OrderByDescending(p => p.PublicationDate).Skip((pageInfo.CurrentPage - 1) * pageInfo.ElementsPerPage).Take(pageInfo.ElementsPerPage).ToListAsync();
        }

        public async Task<IEnumerable<PublicationInfo>> GetBestPublicationsAsync(PageInfo pageInfo)
        {
            var allPublications = repo.Publications.Get().Select(p => new PublicationInfo()
            {
                Id = p.PublicationId,
                Author = p.Author.User.UserName,
                AuthorAvatar = p.Author.Avatar,
                CommentsCount = p.Comments.Count,
                Content = p.Content,
                ContentType = (ContentType)p.ContentType,
                PublicationDate = p.PublicationDate,
                Title = p.Title,
                Rating = p.Rating
            });
            return await allPublications.OrderByDescending(p => p.Rating).Skip((pageInfo.CurrentPage - 1) * pageInfo.ElementsPerPage).Take(pageInfo.ElementsPerPage).ToListAsync();
        }

        public PublicationInfo GetPublication(long publicationId)
        {
            var publication = repo.Publications.Get(publicationId);
            if (publication != null)
            {
                return new PublicationInfo()
                {
                    Id = publication.PublicationId,
                    Author = publication.Author.User.UserName,
                    AuthorAvatar = publication.Author.Avatar,
                    CommentsCount = publication.Comments.Count,
                    Content = publication.Content,
                    ContentType = (ContentType)publication.ContentType,
                    PublicationDate = publication.PublicationDate,
                    Title = publication.Title,
                    Rating = publication.Rating
                };
            }
            return null;
        }

        public async Task<OperationDetails> RatePublication(long publicationId, string username, RateAction rateAction)
        {
            var publication = repo.Publications.Get(publicationId);
            var profile = repo.Profiles.Find(p => p.User.UserName == username).FirstOrDefault();

            if (publication != null && profile != null)
            {
                if (rateAction == RateAction.Up)
                {
                    if (publication.DislikedBy.Contains(profile))
                    {
                        publication.DislikedBy.Remove(profile);
                    }
                    else if (!publication.LikedBy.Contains(profile))
                    {
                        publication.LikedBy.Add(profile);
                    }
                    else
                    {
                        return new OperationDetails(false, "This publication is already rated up");
                    }
                }
                else
                {
                    if (publication.LikedBy.Contains(profile))
                    {
                        publication.LikedBy.Remove(profile);
                    }
                    else if (!publication.DislikedBy.Contains(profile))
                    {
                        publication.DislikedBy.Add(profile);
                    }
                    else
                    {
                        return new OperationDetails(false, "This publication is already rated down");
                    }
                }
                repo.Publications.Update(publication);
                await repo.SaveChangesAsync();
            }

            return new OperationDetails(true, "Successfull");
        }

        #endregion

        #region Comment methods
        public IEnumerable<CommentInfo> GetPublicationComments(long publicationId)
        {
            return repo.Comments.Find(c => c.PublicationId == publicationId).Select(c => new CommentInfo()
            {
                Id = c.CommentId,
                Author = c.Author.User.UserName,
                AuthorAvatar = c.Author.Avatar,
                Content = c.Content,
                PublicationDate = c.PublicationDate,
                Rating = c.Rating
            }).ToList();
        }

        public async Task<OperationDetails> RateComment(long commentId, string username, RateAction rateAction)
        {
            Comment comment = repo.Comments.Get(commentId);
            UserProfile profile = repo.Profiles.Find(p => p.User.UserName == username).FirstOrDefault();

            if (comment != null && profile != null)
            {
                if (rateAction == RateAction.Up)
                {
                    if (comment.DislikedBy.Contains(profile))
                    {
                        comment.DislikedBy.Remove(profile);
                    }
                    else if (!comment.LikedBy.Contains(profile))
                    {
                        comment.LikedBy.Add(profile);
                    }
                    else
                    {
                        return new OperationDetails(false, "This comment is already rated up");
                    }
                }
                else
                {
                    if (comment.LikedBy.Contains(profile))
                    {
                        comment.LikedBy.Remove(profile);
                    }
                    else if (!comment.DislikedBy.Contains(profile))
                    {
                        comment.DislikedBy.Add(profile);
                    }
                    else
                    {
                        return new OperationDetails(false, "This comment is already rated down");
                    }
                }
                repo.Comments.Update(comment);
                await repo.SaveChangesAsync();
            }

            return new OperationDetails(true, "Successfull");
        }

        public CommentInfo GetComment(long commentId)
        {
            var comment = repo.Comments.Get(commentId);
            if (comment != null)
            {
                return new CommentInfo()
                {
                    Id = comment.CommentId,
                    Author = comment.Author.User.UserName,
                    AuthorAvatar = comment.Author.Avatar,
                    Content = comment.Content,
                    Rating = comment.Rating,
                    PublicationDate = comment.PublicationDate
                };
            }
            return null;
        }

        public IEnumerable<CommentInfo> FindComments(Func<CommentInfo, bool> predicate)
        {
            //TODO: Comments filtering
            throw new NotImplementedException();
        }

        public OperationDetails AddComment(CreateCommentData comment, long publicationId)
        {
            Publication publication = repo.Publications.Get(publicationId);
            UserProfile author = repo.Profiles.Find(p => p.User.UserName == comment.Author).FirstOrDefault();
            if (author != null && publication != null)
            {
                repo.Comments.Create(new Comment()
                {
                    Author = author,
                    Publication = publication,
                    Content = comment.Content,
                    PublicationDate = DateTime.Now
                });
                repo.SaveChanges();
                return new OperationDetails(true, "Comment added");
            }
            return new OperationDetails(false, "Can`t find post or author");
        }
        #endregion

        public void Dispose()
        {
            repo.Dispose();
        }
    }
}
