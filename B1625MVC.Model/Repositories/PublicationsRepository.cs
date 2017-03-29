using B1625MVC.Model.Abstract;
using B1625MVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace B1625MVC.Model.Repositories
{
    public class PublicationsRepository : IEntityRepository<Publication, long>
    {
        B1625DbContext _dbContext;

        public PublicationsRepository(B1625DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Publication entity)
        {
            _dbContext.Publications.Add(entity);
        }

        public void Delete(long id)
        {
            var publication = _dbContext.Publications.Find(id);
            if (publication != null)
            {
                _dbContext.Publications.Remove(publication);
            }
        }

        public IEnumerable<Publication> Find(Func<Publication, bool> predicate)
        {
            return _dbContext.Publications.Where(predicate).ToList();
        }

        public Publication Get(long id)
        {
            return _dbContext.Publications.Find(id);
        }

        public IEnumerable<Publication> GetAll()
        {
            return _dbContext.Publications.ToList();
        }

        public void Update(Publication entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
