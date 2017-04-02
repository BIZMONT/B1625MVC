using B1625MVC.Model.Abstract;
using B1625MVC.Model.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace B1625MVC.Model.Repositories
{
    public class ProfileRepository : IEntityRepository<UserProfile, string>
    {
        B1625DbContext _dbContext;

        public ProfileRepository(B1625DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(UserProfile entity)
        {
            _dbContext.UsersProfiles.Add(entity);
        }

        public void Delete(string id)
        {
            var profile = _dbContext.UsersProfiles.Find(id);
            if(profile!=null)
            {
                _dbContext.UsersProfiles.Remove(profile);
            }
        }

        public IQueryable<UserProfile> Find(Expression<Func<UserProfile, bool>> predicate)
        {
            return _dbContext.UsersProfiles.Include(up => up.User).Where(predicate).AsQueryable();
        }

        public UserProfile Get(string id)
        {
            return _dbContext.UsersProfiles.Include(up=>up.User).Where(up=>up.AccountId == id).FirstOrDefault();
        }

        public IQueryable<UserProfile> Get()
        {
            return _dbContext.UsersProfiles.Include(up=>up.User).AsQueryable(); ;
        }

        public void Update(UserProfile entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
