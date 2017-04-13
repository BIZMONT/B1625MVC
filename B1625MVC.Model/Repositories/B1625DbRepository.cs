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
    /// <summary>
    /// Class represents repository to get access to model throuht interface
    /// </summary>
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

        /// <summary>
        /// Property for getting access to user profiles info CRUD class
        /// </summary>
        public IEntityRepository<UserProfile, string> Profiles
        {
            get
            {
                return _profilesRepo;
            }
        }

        /// <summary>
        /// Property for getting access to publications CRUD class
        /// </summary>
        public IEntityRepository<Publication, long> Publications
        {
            get
            {
                return _publicationsRepo;
            }
        }

        /// <summary>
        /// Property for getting access to comments CRUD class
        /// </summary>
        public IEntityRepository<Comment, long> Comments
        {
            get
            {
                return _commentsRepo;
            }
        }

        /// <summary>
        /// Property for getting access to user manager
        /// </summary>
        public UserAccountManager AccountManager
        {
            get
            {
                return _accountManager;
            }
        }

        /// <summary>
        /// Property for getting access to role manager
        /// </summary>
        public UserRoleManager RoleManager
        {
            get
            {
                return _roleManager;
            }
        }

        /// <summary>
        /// IDisposable realization for the safe closure of database connection
        /// </summary>
        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;
        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _accountManager.Dispose();
                    _roleManager.Dispose();
                    _dbContext.Dispose();
                }
            }
        }
        #endregion

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
