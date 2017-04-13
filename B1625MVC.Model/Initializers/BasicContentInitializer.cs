using System;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

using B1625MVC.Model.Entities;
using B1625MVC.Model.Enums;
using B1625MVC.Model.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace B1625MVC.Model.Initializers
{
    /// <summary>
    /// Class represents initializer for model database that adds to database basic data (administrator account, roles ets.)
    /// </summary>
    public class BasicContentInitializer : DropCreateDatabaseIfModelChanges<B1625DbContext>
    {
        protected override void Seed(B1625DbContext context)
        {
            base.Seed(context);

            UserAccountManager accountManager = UserAccountManager.Create(context);
            UserRoleManager roleManager = new UserRoleManager(new RoleStore<UserRole>(context));


            roleManager.Create(new UserRole() { Name = "Administrators" });
            roleManager.Create(new UserRole() { Name = "Moderators" });
            roleManager.Create(new UserRole() { Name = "Users" });

            var admin = accountManager.FindByName("admin");
            if (admin == null)
            {
                admin = new UserAccount() { UserName = "admin", Email = "admin@example.com" };
                var result = accountManager.Create(admin, "adminadmin");
                if (!result.Succeeded)
                {
                }
                accountManager.AddToRole(admin.Id, "Administrators");
                accountManager.AddToRole(admin.Id, "Users");

                var profile = new UserProfile()
                {
                    AccountId = admin.Id,
                    Gender = Gender.Male,
                    RegistrationDate = DateTime.Now
                };
                context.UsersProfiles.Add(profile);
                context.SaveChanges();
            }
        }
    }
}
