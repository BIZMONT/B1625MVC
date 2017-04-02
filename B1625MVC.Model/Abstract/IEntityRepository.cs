using System;
using System.Linq;
using System.Linq.Expressions;

namespace B1625MVC.Model.Abstract
{
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
