using B1625MVC.BLL.Abstract;
using B1625MVC.BLL.DTO;
using B1625MVC.Model.Abstract;
using B1625MVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B1625MVC.BLL.Services
{
    public class ContentService : IContentService
    {
        IRepository _b1625Repo;

        public ContentService(IRepository repository)
        {
            _b1625Repo = repository;
        }

        public OperationDetails CreatePublication(CreatePublicationData publicationData)
        {
            var author = _b1625Repo.Profiles.Find(p => p.User.UserName == publicationData.Author).FirstOrDefault();

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

                _b1625Repo.Publications.Create(newPublication);
                return new OperationDetails(true, "New publication was created successfully");
            }
            return new OperationDetails(false, "Can`t find author in database");
        }

        public IEnumerable<PublicationInfo> FindPublications(Func<PublicationInfo, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PublicationInfo> GetAllPublications()
        {
            return _b1625Repo.Publications.GetAll().Select(p => new PublicationInfo()
            {
                Id = p.PublicationId,
                Author = p.Author.User.UserName,
                AuthorAvatar = p.Author.Avatar,
                CommentsCount = p.Comments.Count,
                Content = p.Content,
                ContentType = (ContentType)Enum.Parse(typeof(ContentType), Enum.GetName(typeof(Model.Enums.ContentType), p.ContentType)),
                PublicationDate = p.PublicationDate,
                Title = p.Title,
                Rating = p.Rating
            });
        }

        public IEnumerable<CommentInfo> GetPublicationComments(long publicationId)
        {
            return _b1625Repo.Comments.Find(c => c.PublicationId == publicationId).Select(c => new CommentInfo()
            {
                Id = c.CommentId,
                Author = c.Author.User.UserName,
                AuthorAvatar = c.Author.Avatar,
                Content = c.Content,
                PublicationDate = c.PublicationDate,
                Rating = c.Rating
            });
        }

        public void Dispose()
        {
            _b1625Repo.Dispose();
        }

        public PublicationInfo GetPublication(long publicationId)
        {
            var publication = _b1625Repo.Publications.Get(publicationId);
            if (publication != null)
            {
                return new PublicationInfo()
                {
                    Id = publication.PublicationId,
                    Author = publication.Author.User.UserName,
                    AuthorAvatar = publication.Author.Avatar,
                    CommentsCount = publication.Comments.Count,
                    Content = publication.Content,
                    ContentType = (ContentType)Enum.Parse(typeof(ContentType), Enum.GetName(typeof(Model.Enums.ContentType), publication.ContentType)),
                    PublicationDate = publication.PublicationDate,
                    Title = publication.Title,
                    Rating = publication.Rating
                };
            }
            return null;
        }

        public async Task<OperationDetails> RatePublication(long publicationId, string username, RateAction rateAction)
        {
            var publication = _b1625Repo.Publications.Get(publicationId);
            var profile = _b1625Repo.Profiles.Find(p => p.User.UserName == username).FirstOrDefault();

            if (publication != null && profile != null)
            {
                if (rateAction == RateAction.Up)
                {
                    if(publication.DislikedBy.Contains(profile))
                    {
                        publication.DislikedBy.Remove(profile);
                    }
                    else if(!publication.LikedBy.Contains(profile))
                    {
                        publication.LikedBy.Add(profile);
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
                }
                _b1625Repo.Publications.Update(publication);
                await _b1625Repo.SaveChangesAsync();
            }

            return new OperationDetails(true, "Successfull");
        }

        public async Task<OperationDetails> RateComment(long commentId, string name, RateAction up)
        {
            throw new NotImplementedException();
        }
    }
}
