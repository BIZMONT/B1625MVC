using B1625MVC.Model.Abstract;
using B1625MVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace B1625MVC.Model.Repositories
{
    public class CommentsRepository : IEntityRepository<Comment, long>
    {
        private B1625DbContext _dbContext;

        public CommentsRepository(B1625DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Comment entity)
        {
            _dbContext.Comments.Add(entity);
        }

        public void Delete(long id)
        {
            var comment = _dbContext.Comments.Find(id);
            if (comment != null)
            {
                _dbContext.Comments.Remove(comment);
            }
        }

        public IEnumerable<Comment> Find(Func<Comment, bool> predicate)
        {
            return _dbContext.Comments.Where(predicate).ToList();
        }

        public Comment Get(long id)
        {
            return _dbContext.Comments.Find(id);
        }

        public IEnumerable<Comment> GetAll()
        {
            return _dbContext.Comments;
        }

        public void Update(Comment entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
