﻿using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

using B1625MVC.Model.Entities;

namespace B1625MVC.Model.EntitiesConfigs
{
    internal class UserAccountConfig : EntityTypeConfiguration<UserAccount>
    {
        public UserAccountConfig()
        {
            HasKey(ua => ua.UserId);
            Property(ua => ua.Username)
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName, 
                    new IndexAnnotation(
                        new System.ComponentModel.DataAnnotations.Schema.IndexAttribute("IX_Username", 0)
                        {
                            IsUnique = true
                        }));
            Property(ua => ua.Email)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName, 
                    new IndexAnnotation(
                        new System.ComponentModel.DataAnnotations.Schema.IndexAttribute("IX_Email", 0)
                        {
                            IsUnique = true
                        }));
        }
    }
}
