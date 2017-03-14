using B1625DbModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1625DbModel.EntitiesConfigs
{
    public class CommentConfig : EntityTypeConfiguration<Comment>
    {
        public CommentConfig()
        {
            HasKey(c => c.CommentId);
            Property(c => c.Content).IsRequired();
        }
    }
}
