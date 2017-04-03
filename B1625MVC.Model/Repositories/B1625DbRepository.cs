using B1625MVC.Model.Abstract;
using System;
using System.Threading.Tasks;
using B1625MVC.Model.Entities;
using B1625MVC.Model.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using B1625MVC.Model.Initializers;

namespace B1625MVC.Model.Repositories
{
    public class B1625DbRepository : IRepository
    {
        private B1625DbContext _dbContext;
        private UserAccountManager _accountManager;
        private UserRoleManager _roleManager;
        private ProfileRepository _profilesRepo;
        private PublicationsRepository _publicationsRepo;
        private CommentsRepository _commentsRepo;

        public B1625DbRepository(string connectionString)
        {
            Database.SetInitializer(new BasicContentInitializer());
            _dbContext = new B1625DbContext(connectionString);

            _accountManager = UserAccountManager.Create(_dbContext);
            _roleManager = new UserRoleManager(new RoleStore<UserRole>(_dbContext));

            _profilesRepo = new ProfileRepository(_dbContext);
            _publicationsRepo = new PublicationsRepository(_dbContext);
            _commentsRepo = new CommentsRepository(_dbContext);
        }

        public IEntityRepository<UserProfile, string> Profiles
        {
            get
            {
                return _profilesRepo;
            }
        }
        public IEntityRepository<Publication, long> Publications
        {
            get
            {
                return _publicationsRepo;
            }
        }
        public IEntityRepository<Comment, long> Comments
        {
            get
            {
                return _commentsRepo;
            }
        }

        public UserAccountManager AccountManager
        {
            get
            {
                return _accountManager;
            }
        }
        public UserRoleManager RoleManager
        {
            get
            {
                return _roleManager;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;
        private void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    _accountManager.Dispose();
                    _roleManager.Dispose();
                    _dbContext.Dispose();
                }
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
