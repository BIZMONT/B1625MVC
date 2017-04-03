using B1625MVC.Model.Entities;
using B1625MVC.Model.Identity;
using B1625MVC.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1625MVC.Model.Abstract
{
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
