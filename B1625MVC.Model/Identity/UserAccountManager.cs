using B1625MVC.Model.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Threading.Tasks;

namespace B1625MVC.Model.Identity
{
    public class UserAccountManager : UserManager<UserAccount>
    {
        public UserAccountManager(IUserStore<UserAccount> store) : base(store)
        {
        }

        public static UserAccountManager Create(DbContext dbContext)
        {
            var manager = new UserAccountManager(new UserStore<UserAccount>(dbContext));

            manager.UserValidator = new UserValidator<UserAccount>(manager)
            {
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = true
            };

            manager.PasswordValidator = new PasswordValidator()
            {
                RequireDigit = false,
                RequiredLength = 8,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false
            };

            return manager;
        }

        public async Task<IdentityResult> ChangePasswordAsync(UserAccount account, string newPassword)
        {
            var newPasswordHash = PasswordHasher.HashPassword(newPassword);
            account.PasswordHash = newPasswordHash;
            return await Task.FromResult(IdentityResult.Success);
        }
    }
}