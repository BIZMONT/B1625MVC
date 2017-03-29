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
    public class BasicContentInitializer : DropCreateDatabaseIfModelChanges<B1625DbContext>
    {
        protected override void Seed(B1625DbContext context)
        {
            base.Seed(context);

            UserAccountManager accountManager = UserAccountManager.Create(context);
            UserRoleManager roleManager = new UserRoleManager(new RoleStore<UserRole>(context));
            MemoryStream memStream = new MemoryStream();


            roleManager.Create(new UserRole() { Name = "Administrators" });
            roleManager.Create(new UserRole() { Name = "Moderators" });
            roleManager.Create(new UserRole() { Name = "Users" });

            var admin = accountManager.FindByName("admin");
            if (admin == null)
            {
                admin = new UserAccount() { UserName = "admin", Email = "admin@example.com" };
                var result  = accountManager.Create(admin, "adminadmin");
                if(!result.Succeeded)
                {
                }
                accountManager.AddToRole(admin.Id, "Administrators");
                accountManager.AddToRole(admin.Id, "Users");

                Resources.AdminAvatar.Save(memStream, ImageFormat.Jpeg);
                var profile = new UserProfile()
                {
                    AccountId = admin.Id,
                    Avatar = memStream.ToArray(),
                    Gender = Gender.Male
                };
                context.UsersProfiles.Add(profile);
                context.SaveChanges();
            }

            memStream = new MemoryStream();
            Resources.Beacon.Save(memStream, ImageFormat.Jpeg);
            var imagePost = new Publication()
            {
                Author = admin.Profile,
                Title = "First image publication",
                ContentType = ContentType.Image,
                Content = memStream.ToArray()
            };
            imagePost.Comments.Add(new Comment()
            {
                Author = admin.Profile,
                Content = "TestComment"
            });

            var textPost = new Publication()
            {
                Author = admin.Profile,
                Title = "First text publication",
                ContentType = ContentType.Text,
                Content = Encoding.Default.GetBytes("Welcome to site!")
            };

            textPost.LikedBy.Add(admin.Profile);

            admin.Profile.Publications.Add(imagePost);
            admin.Profile.Publications.Add(textPost);

            context.SaveChanges();
        }
    }
}
