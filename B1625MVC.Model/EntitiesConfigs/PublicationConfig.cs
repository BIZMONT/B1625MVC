using System.Data.Entity.ModelConfiguration;

using B1625MVC.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace B1625MVC.Model.EntitiesConfigs
{
    internal class PublicationConfig : EntityTypeConfiguration<Publication>
    {
        public PublicationConfig()
        {
            HasKey(p => p.PublicationId);

            Property(p => p.Title).IsRequired().HasMaxLength(64);
            Property(p => p.Content).IsRequired();
            Property(p => p.ContentType).IsRequired();
            Property(p => p.PublicationDate).IsRequired();

            Property(p => p.Rating).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}
