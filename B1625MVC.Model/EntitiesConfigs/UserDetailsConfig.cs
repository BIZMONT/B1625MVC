using System.Data.Entity.ModelConfiguration;

using B1625MVC.Model.Entities;

namespace B1625MVC.Model.EntitiesConfigs
{
    internal class UserDetailsConfig : EntityTypeConfiguration<UserDetails>
    {
        public UserDetailsConfig()
        {
            HasKey(ud => ud.UserId);
        }
    }
}
