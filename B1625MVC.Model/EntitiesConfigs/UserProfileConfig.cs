using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

using B1625MVC.Model.Entities;

namespace B1625MVC.Model.EntitiesConfigs
{
    internal class UserProfileConfig : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileConfig()
        {
            HasKey(up => up.AccountId);

            Property(up => up.Avatar).IsOptional();
            Property(up => up.Gender).IsOptional();
            Property(up => up.RegistrationDate).IsRequired();

            Ignore(up => up.Rating);
            Ignore(up => up.UserName);
        }
    }
}
