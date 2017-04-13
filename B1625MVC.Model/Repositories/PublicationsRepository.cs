using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using B1625MVC.Model.Abstract;
using B1625MVC.Model.Entities;

namespace B1625MVC.Model.Repositories
{
    /// <summary>
    /// Realization of CRUD model of publication for database
    /// </summary>
    public class PublicationsRepository : IEntityRepository<Publication, long>
    {
        B1625DbContext _dbContext;

        public PublicationsRepository(B1625DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Method for adding new publication to database
        /// </summary>
        /// <param name="entity">publication data</param>
        public void Create(Publication entity)
        {
            _dbContext.Publications.Add(entity);
        }

        /// <summary>
        /// Method for deletin publication through id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(long id)
        {
            var publication = _dbContext.Publications.Find(id);
            if (publication != null)
            {
                _dbContext.Publications.Remove(publication);
            }
        }

        /// <summary>
        /// Method for getting query for publications with filter
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<Publication> Find(Expression<Func<Publication, bool>> predicate)
        {
            return _dbContext.Publications.Where(predicate).AsQueryable();
        }

        /// <summary>
        /// Method for getting query for concrete publication
        /// </summary>
        /// <param name="id">Publication id</param>
        /// <returns></returns>
        public Publication Get(long id)
        {
            return _dbContext.Publications.Find(id);
        }

        /// <summary>
        /// Method for getting query for all publications
        /// </summary>
        /// <returns></returns>
        public IQueryable<Publication> Get()
        {
            return _dbContext.Publications.AsQueryable();
        }

        /// <summary>
        /// Method for updating concrete publication data in database
        /// </summary>
        /// <param name="entity">Puplication that will be updated</param>
        public void Update(Publication entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
