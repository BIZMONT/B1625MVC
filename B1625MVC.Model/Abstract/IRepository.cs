using System;
using System.Threading.Tasks;

using B1625MVC.Model.Entities;
using B1625MVC.Model.Identity;

namespace B1625MVC.Model.Abstract
{
    /// <summary>
    /// Interface for unit of work pattern realization
    /// </summary>
    public interface IRepository : IDisposable
    {
        IEntityRepository<UserProfile, string> Profiles { get; }
        IEntityRepository<Publication, long> Publications { get; }
        IEntityRepository<Comment, long> Comments { get; }
        UserAccountManager AccountManager { get; }
        UserRoleManager RoleManager { get; }

        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
