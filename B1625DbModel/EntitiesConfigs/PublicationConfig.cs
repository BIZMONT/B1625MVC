using B1625DbModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1625DbModel.EntitiesConfigs
{
    internal class PublicationConfig : EntityTypeConfiguration<Publication>
    {
        public PublicationConfig()
        {
            HasKey(p => p.PublicationId);

            Property(p => p.Title).IsRequired().HasMaxLength(64);
            Property(p => p.Content).IsRequired();
            Property(p => p.ContentType).IsRequired();

            Ignore(p => p.Rating);
        }
    }
}
