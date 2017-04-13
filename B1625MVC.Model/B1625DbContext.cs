using System.Data.Entity;

using B1625MVC.Model.Entities;
using B1625MVC.Model.EntitiesConfigs;
using Microsoft.AspNet.Identity.EntityFramework;

namespace B1625MVC.Model
{
    /// <summary>
    /// Class represents database model context
    /// </summary>
    public class B1625DbContext : IdentityDbContext
    {
        public B1625DbContext() : this("B1625Db")
        {
        }

        public B1625DbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public static B1625DbContext Create()
        {
            return new B1625DbContext();
        }

        public DbSet<UserProfile> UsersProfiles { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserProfileConfig());
            modelBuilder.Configurations.Add(new PublicationConfig());
            modelBuilder.Configurations.Add(new CommentConfig());

            modelBuilder.Entity<UserAccount>().HasRequired(ua => ua.Profile).WithRequiredPrincipal(ud => ud.User).WillCascadeOnDelete(true);

            modelBuilder.Entity<UserProfile>().HasMany(up => up.Publications).WithRequired(p => p.Author).HasForeignKey(p=>p.AuthorId).WillCascadeOnDelete(false);
            modelBuilder.Entity<UserProfile>().HasMany(up => up.Comments).WithRequired(c => c.Author).HasForeignKey(p => p.AuthorId).WillCascadeOnDelete(false);

            modelBuilder.Entity<UserProfile>().HasMany(ua => ua.LikedPublications).WithMany(p => p.LikedBy).Map(lp =>
            {
                lp.MapLeftKey("ProfileId");
                lp.MapRightKey("PublicationId");
                lp.ToTable("PublicationsLikes");
            });
            modelBuilder.Entity<UserProfile>().HasMany(ua => ua.DislikedPublications).WithMany(p => p.DislikedBy).Map(lp =>
            {
                lp.MapLeftKey("ProfileId");
                lp.MapRightKey("PublicationId");
                lp.ToTable("PublicationsDislikes");
            });
            modelBuilder.Entity<UserProfile>().HasMany(ua => ua.LikedComments).WithMany(p => p.LikedBy).Map(lp =>
            {
                lp.MapLeftKey("ProfileId");
                lp.MapRightKey("CommentId");
                lp.ToTable("CommentsLikes");
            });
            modelBuilder.Entity<UserProfile>().HasMany(ua => ua.DislikedComments).WithMany(p => p.DislikedBy).Map(lp =>
            {
                lp.MapLeftKey("ProfileId");
                lp.MapRightKey("CommentId");
                lp.ToTable("CommentsDislikes");
            });

            modelBuilder.Entity<Publication>().HasMany(p => p.Comments).WithRequired(c => c.Publication).HasForeignKey(c => c.PublicationId).WillCascadeOnDelete(true);
        }
    }
}
