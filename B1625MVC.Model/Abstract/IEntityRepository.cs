using System;
using System.Linq;
using System.Linq.Expressions;

namespace B1625MVC.Model.Abstract
{
    /// <summary>
    /// Interface for CRUD
    /// </summary>
    /// <typeparam name="TEntity">Type of model</typeparam>
    /// <typeparam name="TPrimaryKey">Type of primary key in model</typeparam>
    public interface IEntityRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        TEntity Get(TPrimaryKey id);
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Create(TEntity entity);
        void Delete(TPrimaryKey id);
        void Update(TEntity entity);
    }
}
