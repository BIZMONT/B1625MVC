using B1625DbModel.Entities;
using B1625DbModel.EntitiesConfigs;
using System.Data.Entity;

namespace B1625DbModel
{
    public class B1625DbContext : DbContext
    {
        public B1625DbContext() : this("B1625Db")
        {

        }
        public B1625DbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {

        }

        public DbSet<UserAccount> Accounts { get; set; }
        public DbSet<UserDetails> UsersDEtails { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserAccountConfig());
            modelBuilder.Configurations.Add(new UserDetailsConfig());
            modelBuilder.Configurations.Add(new PublicationConfig());
            modelBuilder.Configurations.Add(new CommentConfig());

            modelBuilder.Entity<UserAccount>().HasMany(ua => ua.Publications).WithRequired(p => p.Author).HasForeignKey(p => p.AuthorId).WillCascadeOnDelete();
            modelBuilder.Entity<UserAccount>().HasMany(ua => ua.Comments).WithRequired(c => c.Author).HasForeignKey(c => c.AuthorId).WillCascadeOnDelete();
            modelBuilder.Entity<UserAccount>().HasOptional(ua => ua.Details).WithRequired(ud => ud.User);
            modelBuilder.Entity<Publication>().HasMany(p => p.Comments).WithRequired(c => c.Publication).HasForeignKey(c => c.PublicationId).WillCascadeOnDelete();
        }
    }
}
