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
        private ProfileRepository _profileRepo;

        public B1625DbRepository(string connectionString)
        {
            Database.SetInitializer(new BasicContentInitializer());
            _dbContext = new B1625DbContext(connectionString);

            _accountManager = UserAccountManager.Create(_dbContext);
            _roleManager = new UserRoleManager(new RoleStore<UserRole>(_dbContext));

            _profileRepo = new ProfileRepository(_dbContext);
        }

        public IEntityRepository<UserProfile, string> Profiles
        {
            get
            {
                return _profileRepo;
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

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
