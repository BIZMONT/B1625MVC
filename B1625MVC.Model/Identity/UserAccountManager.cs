using B1625MVC.Model.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Threading.Tasks;

namespace B1625MVC.Model.Identity
{
    /// <summary>
    /// Class for managing users trough ASP.NET Identity
    /// </summary>
    public class UserAccountManager : UserManager<UserAccount>
    {
        public UserAccountManager(IUserStore<UserAccount> store) : base(store)
        {
        }

        /// <summary>
        /// Create instance of this class with basic settings
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static UserAccountManager Create(DbContext dbContext)
        {
            var manager = new UserAccountManager(new UserStore<UserAccount>(dbContext));

            manager.UserValidator = new UserValidator<UserAccount>(manager) //add user validator to manager
            {
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = true
            };

            manager.PasswordValidator = new PasswordValidator() //add password validator to manager
            {
                RequireDigit = false,
                RequiredLength = 8,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false
            };

            manager.UserTokenProvider = new EmailTokenProvider<UserAccount>();

            return manager;
        }

        /// <summary>
        /// Change password without passing old password
        /// </summary>
        /// <param name="account"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<IdentityResult> ChangePasswordAsync(UserAccount account, string newPassword)
        {
            var newPasswordHash = PasswordHasher.HashPassword(newPassword);
            account.PasswordHash = newPasswordHash;
            return await Task.FromResult(IdentityResult.Success);
        }
    }
}