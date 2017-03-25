using B1625MVC.Model.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace B1625MVC.Model.Identity
{
    public class UserAccountManager : UserManager<UserAccount>
    {
        public UserAccountManager(IUserStore<UserAccount> store) : base(store)
        {
        }

        public static UserAccountManager Create(IdentityFactoryOptions<UserAccountManager> options, IOwinContext context)
        {
            var dbContext = context.Get<B1625DbContext>();
            var manager = new UserAccountManager(new UserStore<UserAccount>());

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
    }
}