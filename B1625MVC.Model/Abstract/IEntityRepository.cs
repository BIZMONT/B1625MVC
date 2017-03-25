using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1625MVC.Model.Abstract
{
    public interface IEntityRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        TEntity Get(TPrimaryKey id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Func<TEntity, bool> predicate);

        void Create(TEntity entity);
        void Delete(TPrimaryKey id);
        void Update(TEntity entity);
    }
}
