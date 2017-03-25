using B1625MVC.Model.Entities;
using Microsoft.AspNet.Identity;

namespace B1625MVC.Model.Identity
{
    public class UserRoleManager : RoleManager<UserRole>
    {
        public UserRoleManager(IRoleStore<UserRole, string> store) : base(store)
        {
        }
    }
}
