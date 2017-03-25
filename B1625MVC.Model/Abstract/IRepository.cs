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
        UserAccountManager AccountManager { get; }
        UserRoleManager RoleManager { get; }

        Task SaveChangesAsync();
    }
}
