﻿using System.Data.Entity.ModelConfiguration;

using B1625MVC.Model.Entities;

namespace B1625MVC.Model.EntitiesConfigs
{
    internal class CommentConfig : EntityTypeConfiguration<Comment>
    {
        public CommentConfig()
        {
            HasKey(c => c.CommentId);

            Property(c => c.Content).IsRequired();
            Property(c => c.PublicationDate).IsRequired();
            Property(c => c.Rating).IsOptional();
        }
    }
}
