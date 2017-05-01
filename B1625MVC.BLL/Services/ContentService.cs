using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.BLL.DTO.Enums;
using B1625MVC.Model.Abstract;
using B1625MVC.Model.Entities;
using B1625MVC.BLL.DTO.ContentData.CommentData;
using B1625MVC.BLL.DTO.ContentData.PublicationData;

namespace B1625MVC.BLL.Services
{
    public class ContentService : IContentService
    {
        IRepository repo;

        public ContentService(IRepository repository)
        {
            repo = repository;
        }

        private IQueryable<PublicationInfo> GetAllPublications()
        {
            return repo.Publications.Get().Select(p => new PublicationInfo()
            {
                Id = p.PublicationId,
                Author = p.Author.User != null ? p.Author.User.UserName : "Unknown",
                AuthorAvatar = p.Author.Avatar,
                CommentsCount = p.Comments.Count,
                Content = p.Content,
                ContentType = (ContentType)p.ContentType,
                PublicationDate = p.PublicationDate,
                Title = p.Title,
                Rating = p.Rating
            });
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

        public IEnumerable<PublicationInfo> FindPublications(Expression<Func<PublicationInfo, bool>> predicate, PageInfo pageInfo)
        {
            var allPublications = GetAllPublications();
            int publicationsCount = allPublications.Count();
            pageInfo.TotalPages = publicationsCount / pageInfo.ElementsPerPage + (publicationsCount % pageInfo.ElementsPerPage > 0 ? 1 : 0);
            return allPublications.Where(predicate).OrderBy(p => p.PublicationDate).Skip((pageInfo.CurrentPage - 1) * pageInfo.ElementsPerPage).Take(pageInfo.ElementsPerPage).ToList();
        }

        public async Task<IEnumerable<PublicationInfo>> GetPublicationsAsync(PageInfo pageInfo)
        {
            var allPublications = GetAllPublications();

            int publicationsCount = allPublications.Count();
            pageInfo.TotalPages = publicationsCount / pageInfo.ElementsPerPage + (publicationsCount % pageInfo.ElementsPerPage > 0 ? 1 : 0);
            return await allPublications.OrderByDescending(p => p.PublicationDate).Skip((pageInfo.CurrentPage - 1) * pageInfo.ElementsPerPage).Take(pageInfo.ElementsPerPage).ToListAsync();
        }

        public async Task<IEnumerable<PublicationInfo>> GetBestPublicationsAsync(PageInfo pageInfo)
        {
            var allPublications = GetAllPublications();

            int publicationsCount = allPublications.Count();
            pageInfo.TotalPages = publicationsCount / pageInfo.ElementsPerPage + (publicationsCount % pageInfo.ElementsPerPage > 0 ? 1 : 0);
            return await allPublications.OrderByDescending(p => p.Rating).Skip((pageInfo.CurrentPage - 1) * pageInfo.ElementsPerPage).Take(pageInfo.ElementsPerPage).ToListAsync();
        }

        public async Task<IEnumerable<PublicationInfo>> GetHotPublicationsAsync(PageInfo pageInfo)
        {
            var hotest = GetAllPublications();

            int publicationsCount = hotest.Count();
            pageInfo.TotalPages = publicationsCount / pageInfo.ElementsPerPage + (publicationsCount % pageInfo.ElementsPerPage > 0 ? 1 : 0);

            return await hotest.Select(h => new PublicationInfo()
            {
                Id = h.Id,
                Author = h.Author,
                AuthorAvatar = h.AuthorAvatar,
                CommentsCount = h.CommentsCount,
                Content = h.Content,
                ContentType = h.ContentType,
                PublicationDate = h.PublicationDate,
                Title = h.Title,
                Rating = h.Rating
            }).OrderByDescending(p => p.CommentsCount).Skip((pageInfo.CurrentPage - 1) * pageInfo.ElementsPerPage).Take(pageInfo.ElementsPerPage).ToListAsync();
        }

        public PublicationInfo GetPublication(long publicationId)
        {
            var publication = repo.Publications.Get(publicationId);
            if (publication != null)
            {
                return new PublicationInfo()
                {
                    Id = publication.PublicationId,
                    Author = publication.Author.User != null ? publication.Author.User.UserName : "Unknown",
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

        public PublicationInfo GetBestPublication()
        {
            return repo.Publications.Get().OrderByDescending(p => p.Rating).Select(publication => new PublicationInfo()
            {
                Id = publication.PublicationId,
                Author = publication.Author.User != null ? publication.Author.User.UserName : "Unknown",
                AuthorAvatar = publication.Author.Avatar,
                CommentsCount = publication.Comments.Count,
                Content = publication.Content,
                ContentType = (ContentType)publication.ContentType,
                PublicationDate = publication.PublicationDate,
                Title = publication.Title,
                Rating = publication.Rating
            }).First();
        }

        public async Task<OperationDetails> RatePublication(long publicationId, string username, RateAction rateAction)
        {
            var publication = repo.Publications.Get(publicationId);
            var profile = repo.Profiles.Find(p => p.User.UserName == username).FirstOrDefault();

            if (publication != null && profile != null)
            {
                if (publication.Author.AccountId == profile.AccountId)
                {
                    return new OperationDetails(false, "Can`t rate own publication");
                }

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
                return new OperationDetails(true, "Successfull");
            }

            return new OperationDetails(false, "Publication not found");
        }

        public OperationDetails DeletePublication(long publicationId)
        {
            repo.Publications.Delete(publicationId);
            int res = repo.SaveChanges();
            if (res > 0)
            {
                return new OperationDetails(true, "Publication deleted");
            }
            return new OperationDetails(false, "Can`t delete publication");
        }

        public OperationDetails EditPublication(EditPublicationData publicationData)
        {
            var publication = repo.Publications.Find(p => p.PublicationId == publicationData.Id).FirstOrDefault();
            if(publication != null)
            {
                publication.Title = publicationData.Title;
                publication.Content = publicationData.Content;
                repo.Publications.Update(publication);
                repo.SaveChanges();
                return new OperationDetails(true, "Publication updated");
            }
            return new OperationDetails(false, "Publication not found");
        }

        public OperationDetails DeleteComment(long commentId)
        {
            repo.Comments.Delete(commentId);
            int res = repo.SaveChanges();
            if (res > 0)
            {
                return new OperationDetails(true, "Comment deleted");
            }
            return new OperationDetails(false, "Can`t delete comment");
        }
        #endregion

        #region Comment methods
        public IEnumerable<CommentInfo> GetPublicationComments(long publicationId)
        {
            return repo.Comments.Find(c => c.PublicationId == publicationId).Select(c => new CommentInfo()
            {
                Id = c.CommentId,
                Author = c.Author.User != null ? c.Author.User.UserName : "Unknown",
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
                if (comment.Author.AccountId == profile.AccountId)
                {
                    return new OperationDetails(false, "Can`t rate own comment");
                }

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
                return new OperationDetails(true, "Successfull");
            }

            return new OperationDetails(false, "Comment not found");
        }

        public CommentInfo GetComment(long commentId)
        {
            var comment = repo.Comments.Get(commentId);
            if (comment != null)
            {
                return new CommentInfo()
                {
                    Id = comment.CommentId,
                    Author = comment.Author.User != null ? comment.Author.User.UserName : "Unknown",
                    AuthorAvatar = comment.Author.Avatar,
                    Content = comment.Content,
                    Rating = comment.Rating,
                    PublicationDate = comment.PublicationDate
                };
            }
            return null;
        }

        public IEnumerable<CommentInfo> FindComments(Expression<Func<CommentInfo, bool>> predicate, PageInfo pageInfo)
        {
            var allCommets = repo.Comments.Get().Select(comment => new CommentInfo()
            {
                Id = comment.CommentId,
                Author = comment.Author.User != null ? comment.Author.User.UserName : "Unknown",
                AuthorAvatar = comment.Author.Avatar,
                Content = comment.Content,
                Rating = comment.Rating,
                PublicationDate = comment.PublicationDate
            });

            int publicationsCount = allCommets.Count();
            pageInfo.TotalPages = publicationsCount / pageInfo.ElementsPerPage + (publicationsCount % pageInfo.ElementsPerPage > 0 ? 1 : 0);
            return allCommets.Where(predicate).OrderBy(p => p.PublicationDate).Skip((pageInfo.CurrentPage - 1) * pageInfo.ElementsPerPage).Take(pageInfo.ElementsPerPage).ToList();

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

        public CommentInfo GetBestComment()
        {
            return repo.Comments.Get().OrderByDescending(c => c.Rating).Select(comment => new CommentInfo()
            {
                Id = comment.CommentId,
                Author = comment.Author.User != null ? comment.Author.User.UserName : "Unknown",
                AuthorAvatar = comment.Author.Avatar,
                Content = comment.Content,
                Rating = comment.Rating,
                PublicationDate = comment.PublicationDate
            }).FirstOrDefault();
        }
        #endregion

        public IEnumerable<UserInfo> GetTopUsers(int count)
        {
            return repo.Profiles.Get().OrderByDescending(u => u.Rating).Take(count).Select(u => new UserInfo()
            {
                UserName = u.User.UserName,
                Rating = u.Rating
            }).ToList();
        }

        public void Dispose()
        {
            repo.Dispose();
        }


    }
}
