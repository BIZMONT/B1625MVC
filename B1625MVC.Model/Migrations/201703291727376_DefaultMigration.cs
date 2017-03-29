namespace B1625MVC.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Long(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        PublicationDate = c.DateTime(nullable: false),
                        Rating = c.Int(nullable: false, defaultValue: 0),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        PublicationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.UserProfiles", t => t.AuthorId)
                .ForeignKey("dbo.Publications", t => t.PublicationId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.PublicationId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        AccountId = c.String(nullable: false, maxLength: 128),
                        Avatar = c.Binary(),
                        Gender = c.Int(),
                        RegistrationDate = c.DateTime(nullable: false),
                        Rating = c.Int(nullable: false, defaultValue: 0),
                    })
                .PrimaryKey(t => t.AccountId)
                .ForeignKey("dbo.AspNetUsers", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Publications",
                c => new
                    {
                        PublicationId = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 64),
                        Content = c.Binary(nullable: false),
                        ContentType = c.Int(nullable: false),
                        PublicationDate = c.DateTime(nullable: false),
                        Rating = c.Int(nullable: false, defaultValue: 0),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.PublicationId)
                .ForeignKey("dbo.UserProfiles", t => t.AuthorId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.CommentsDislikes",
                c => new
                    {
                        ProfileId = c.String(nullable: false, maxLength: 128),
                        CommentId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProfileId, t.CommentId })
                .ForeignKey("dbo.UserProfiles", t => t.ProfileId, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .Index(t => t.ProfileId)
                .Index(t => t.CommentId);
            
            CreateTable(
                "dbo.PublicationsDislikes",
                c => new
                    {
                        ProfileId = c.String(nullable: false, maxLength: 128),
                        PublicationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProfileId, t.PublicationId })
                .ForeignKey("dbo.UserProfiles", t => t.ProfileId, cascadeDelete: true)
                .ForeignKey("dbo.Publications", t => t.PublicationId, cascadeDelete: true)
                .Index(t => t.ProfileId)
                .Index(t => t.PublicationId);
            
            CreateTable(
                "dbo.CommentsLikes",
                c => new
                    {
                        ProfileId = c.String(nullable: false, maxLength: 128),
                        CommentId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProfileId, t.CommentId })
                .ForeignKey("dbo.UserProfiles", t => t.ProfileId, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .Index(t => t.ProfileId)
                .Index(t => t.CommentId);
            
            CreateTable(
                "dbo.PublicationsLikes",
                c => new
                    {
                        ProfileId = c.String(nullable: false, maxLength: 128),
                        PublicationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProfileId, t.PublicationId })
                .ForeignKey("dbo.UserProfiles", t => t.ProfileId, cascadeDelete: true)
                .ForeignKey("dbo.Publications", t => t.PublicationId, cascadeDelete: true)
                .Index(t => t.ProfileId)
                .Index(t => t.PublicationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.UserProfiles", "AccountId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Publications", "AuthorId", "dbo.UserProfiles");
            DropForeignKey("dbo.PublicationsLikes", "PublicationId", "dbo.Publications");
            DropForeignKey("dbo.PublicationsLikes", "ProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.CommentsLikes", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.CommentsLikes", "ProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.PublicationsDislikes", "PublicationId", "dbo.Publications");
            DropForeignKey("dbo.PublicationsDislikes", "ProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.Comments", "PublicationId", "dbo.Publications");
            DropForeignKey("dbo.CommentsDislikes", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.CommentsDislikes", "ProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.UserProfiles");
            DropIndex("dbo.PublicationsLikes", new[] { "PublicationId" });
            DropIndex("dbo.PublicationsLikes", new[] { "ProfileId" });
            DropIndex("dbo.CommentsLikes", new[] { "CommentId" });
            DropIndex("dbo.CommentsLikes", new[] { "ProfileId" });
            DropIndex("dbo.PublicationsDislikes", new[] { "PublicationId" });
            DropIndex("dbo.PublicationsDislikes", new[] { "ProfileId" });
            DropIndex("dbo.CommentsDislikes", new[] { "CommentId" });
            DropIndex("dbo.CommentsDislikes", new[] { "ProfileId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Publications", new[] { "AuthorId" });
            DropIndex("dbo.UserProfiles", new[] { "AccountId" });
            DropIndex("dbo.Comments", new[] { "PublicationId" });
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            DropTable("dbo.PublicationsLikes");
            DropTable("dbo.CommentsLikes");
            DropTable("dbo.PublicationsDislikes");
            DropTable("dbo.CommentsDislikes");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Publications");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.Comments");
        }
    }
}
