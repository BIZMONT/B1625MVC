namespace B1625MVC.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Init : DbMigration
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
                    Rating = c.Int(),
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
                    Rating = c.Int(),
                })
                .PrimaryKey(t => t.AccountId);

            CreateTable(
                "dbo.Publications",
                c => new
                {
                    PublicationId = c.Long(nullable: false, identity: true),
                    Title = c.String(nullable: false, maxLength: 64),
                    Content = c.Binary(nullable: false),
                    ContentType = c.Int(nullable: false),
                    PublicationDate = c.DateTime(nullable: false),
                    Rating = c.Int(),
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
                .ForeignKey("dbo.UserProfiles", t => t.Id)
                .Index(t => t.Id)
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

            #region functions
            Sql("CREATE OR ALTER FUNCTION GetPublicationRating (@PublicationId bigint) " +
                "RETURNS int " +
                "BEGIN " +
                    "DECLARE @Dislikes INTEGER " +
                    "DECLARE @Likes INTEGER " +
                    "SET @Likes = (SELECT Count(*) FROM [PublicationsLikes] WHERE [PublicationId] = @PublicationId) " +
                    "SET @Dislikes = (SELECT Count(*) FROM [PublicationsDislikes] WHERE [PublicationId] = @PublicationId) " +
                    "RETURN @Likes - @Dislikes " +
                "END");

            Sql("CREATE OR ALTER FUNCTION GetCommentRating (@CommentId bigint) " +
                "RETURNS int " +
                "BEGIN " +
                    "DECLARE @Dislikes INTEGER " +
                    "DECLARE @Likes INTEGER " +
                    "SET @Likes = (SELECT Count(*) FROM [CommentsLikes] WHERE [CommentId] = @CommentId) " +
                    "SET @Dislikes = (SELECT Count(*) FROM [CommentsDislikes] WHERE [CommentId] = @CommentId) " +
                    "RETURN @Likes - @Dislikes " +
                "END");
            #endregion

            #region Publication triggers
            Sql("CREATE TRIGGER AddedToPublicationsLikes " +
                "ON [dbo].[PublicationsLikes] " +
                "AFTER INSERT AS " +
                "BEGIN " +
                    "DECLARE @PublicationId bigint " +
                    "SELECT @PublicationId = ins.PublicationId FROM inserted ins " +
                    "DECLARE @Rating int " +
                    "SET @Rating = dbo.GetPublicationRating(@PublicationId) " +
                    "UPDATE Publications " +
                        "SET [Rating] = @Rating " +
                        "WHERE [PublicationId] = @PublicationId " +
                "END");

            Sql("CREATE TRIGGER RemovedFromPublicationsLikes " +
                "ON [dbo].[PublicationsLikes] FOR DELETE AS " +
                "BEGIN " +
                    "DECLARE @PublicationId bigint " +
                    "SELECT @PublicationId = del.PublicationId FROM deleted del " +
                    "DECLARE @Rating int " +
                    "SET @Rating = dbo.GetPublicationRating(@PublicationId) " +
                    "UPDATE Publications " +
                        "SET [Rating] = @Rating " +
                        "WHERE [PublicationId] = @PublicationId " +
                "END");

            Sql("CREATE TRIGGER AddedToPublicationsDislikes " +
                "ON [dbo].[PublicationsDislikes] AFTER INSERT AS " +
                "BEGIN " +
                    "DECLARE @PublicationId bigint " +
                    "SELECT @PublicationId = ins.PublicationId FROM INSERTED ins " +
                    "DECLARE @Rating int " +
                    "SET @Rating = dbo.GetPublicationRating(@PublicationId) " +
                    "UPDATE Publications " +
                        "SET [Rating] = @Rating " +
                        "WHERE [PublicationId] = @PublicationId " +
                "END");

            Sql("CREATE TRIGGER RemovedFromPublicationsDislikes " +
                "ON [dbo].[PublicationsDislikes] FOR DELETE AS " +
                "BEGIN " +
                    "DECLARE @PublicationId bigint " +
                    "SELECT @PublicationId = del.PublicationId FROM deleted del " +
                    "DECLARE @Rating int " +
                    "SET @Rating = dbo.GetPublicationRating(@PublicationId) " +
                    "UPDATE Publications " +
                        "SET[Rating] = @Rating " +
                        "WHERE[PublicationId] = @PublicationId " +
                "END");
            #endregion

            #region Comment triggers
            Sql("CREATE TRIGGER AddedToCommentsLikes " +
                "ON [dbo].[CommentsLikes] AFTER INSERT AS " +
                "BEGIN " +
                    "DECLARE @CommentId bigint " +
                    "SELECT @CommentId = ins.CommentId FROM INSERTED ins " +
                    "DECLARE @Rating int " +
                    "SET @Rating = dbo.GetCommentRating(@CommentId) " +
                    "UPDATE Comments " +
                        "SET [Rating] = @Rating " +
                        "WHERE [CommentId] = @CommentId " +
                "END");

            Sql("CREATE TRIGGER RemovedFromCommentsLikes " +
                "ON [dbo].[CommentsLikes] FOR DELETE AS " +
                "BEGIN " +
                    "DECLARE @CommentId bigint " +
                    "SELECT @CommentId = del.CommentId FROM DELETED del " +
                    "DECLARE @Rating int " +
                    "SET @Rating = dbo.GetCommentRating(@CommentId) " +
                    "UPDATE Comments " +
                        "SET [Rating] = @Rating " +
                        "WHERE [CommentId] = @CommentId " +
                "END");

            Sql("CREATE TRIGGER AddedToCommentsDislikes " +
                "ON [dbo].[CommentsDislikes] AFTER INSERT AS " +
                "BEGIN " +
                    "DECLARE @CommentId bigint " +
                    "SELECT @CommentId = ins.CommentId FROM INSERTED ins " +
                    "DECLARE @Rating int " +
                    "SET @Rating = dbo.GetCommentRating(@CommentId) " +
                    "UPDATE Comments " +
                        "SET [Rating] = @Rating " +
                        "WHERE [CommentId] = @CommentId " +
                "END");

            Sql("CREATE TRIGGER RemovedFromCommentsDislikes " +
                "ON [dbo].[CommentsDislikes] FOR DELETE AS " +
                "BEGIN " +
                    "DECLARE @CommentId bigint " +
                    "SELECT @CommentId = del.CommentId FROM deleted del " +
                    "DECLARE @Rating int " +
                    "SET @Rating = dbo.GetCommentRating(@CommentId) " +
                    "UPDATE Comments " +
                        "SET [Rating] = @Rating " +
                        "WHERE [CommentId] = @CommentId " +
                "END");
            #endregion

            #region Profile trigers
            Sql("CREATE TRIGGER PublicationRatingChanges " +
                "ON [dbo].[Publications] AFTER INSERT, UPDATE " +
                "AS IF UPDATE(Rating) " +
                "BEGIN " +
                    "DECLARE @ProfileId nvarchar(MAX) " +
                    "SELECT @ProfileId = ins.AuthorId FROM INSERTED ins " +
                    "DECLARE @RatingFromPublications int " +
                    "DECLARE @RatingFromComments int " +
                    "SET @RatingFromPublications = (SELECT SUM(Rating) FROM [dbo].[Publications] WHERE [AuthorId] = @ProfileId) " +
                    "SET @RatingFromComments = (SELECT SUM(Rating) FROM [dbo].[Comments] WHERE [AuthorId] = @ProfileId) " +
                    "SET @RatingFromPublications = CASE WHEN @RatingFromPublications IS NULL THEN 0 ELSE @RatingFromPublications END " +
                    "SET @RatingFromComments = CASE WHEN @RatingFromComments IS NULL THEN 0 ELSE @RatingFromComments END " +
                    "UPDATE [dbo].[UserProfiles] " +
                        "SET [Rating] = @RatingFromPublications + @RatingFromComments " +
                        "WHERE [AccountId] = @ProfileId " +
                "END");
            Sql("CREATE TRIGGER CommentRatingChanges " +
                "ON [dbo].[Comments] AFTER INSERT, UPDATE " +
                "AS IF UPDATE(Rating) " +
                "BEGIN " +
                    "DECLARE @ProfileId nvarchar(MAX) " +
                    "SELECT @ProfileId = ins.AuthorId FROM INSERTED ins " +
                    "DECLARE @RatingFromPublications int " +
                    "DECLARE @RatingFromComments int " +
                    "SET @RatingFromPublications = (SELECT SUM(Rating) FROM [dbo].[Publications] WHERE [AuthorId] = @ProfileId) " +
                    "SET @RatingFromComments = (SELECT SUM(Rating) FROM [dbo].[Comments] WHERE [AuthorId] = @ProfileId) " +
                    "SET @RatingFromPublications = CASE WHEN @RatingFromPublications IS NULL THEN 0 ELSE @RatingFromPublications END " +
                    "SET @RatingFromComments = CASE WHEN @RatingFromComments IS NULL THEN 0 ELSE @RatingFromComments END " +
                    "UPDATE [dbo].[UserProfiles] " +
                        "SET [Rating] = @RatingFromPublications + @RatingFromComments " +
                        "WHERE [AccountId] = @ProfileId " +
                "END");
            #endregion

        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUsers", "Id", "dbo.UserProfiles");
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
            DropIndex("dbo.AspNetUsers", new[] { "Id" });
            DropIndex("dbo.Publications", new[] { "AuthorId" });
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

            Sql("DROP FUNCTION GetPublicationRating");
            Sql("DROP FUNCTION GetCommentRating");
            Sql("DROP TRIGGER AddedToPublicationsLikes");
            Sql("DROP TRIGGER RemovedFromPublicationsLikes");
            Sql("DROP TRIGGER AddedToPublicationsDislikes");
            Sql("DROP TRIGGER RemovedFromPublicationsDislikes");
            Sql("DROP TRIGGER AddedToCommentsLikes");
            Sql("DROP TRIGGER RemovedFromCommentsLikes");
            Sql("DROP TRIGGER AddedToCommentsDislikes");
            Sql("DROP TRIGGER RemovedFromCommentsDislikes");
            Sql("DROP TRIGGER PublicationRatingChanges");
            Sql("DROP TRIGGER CommentRatingChanges");
        }
    }
}
