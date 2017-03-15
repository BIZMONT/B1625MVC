using B1625DbModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1625DbModel.Initializers
{
    public class BasicContentInitializer : DropCreateDatabaseIfModelChanges<B1625DbContext>
    {
        protected override void Seed(B1625DbContext context)
        {
            base.Seed(context);

            MemoryStream memStream = new MemoryStream();

            Resources.AdminAvatar.Save(memStream, ImageFormat.Jpeg);
            var admin = new UserAccount() { Username = "admin", Email = "admin@example.com", Details = new UserDetails() { Gender = Gender.Male, Avatar = memStream.ToArray() } };
            var someUser1 = new UserAccount() { Username = "somebody", Email = "somebody@example.com", Details = new UserDetails() };
            var someUser2 = new UserAccount() { Username = "nobody", Email = "nobody@example.com", Details = new UserDetails() };
            var someUser3 = new UserAccount() { Username = "anybody", Email = "anybody@example.com", Details = new UserDetails() };

            memStream = new MemoryStream();
            Resources.Beacon.Save(memStream, ImageFormat.Jpeg);
            var post1 = new Publication() { Title = "Nice image", Author = admin, ContentType = ContentType.Image, Content = memStream.ToArray(), PublicationDate = DateTime.Now.AddDays(-1) };
            post1.LikedBy.Add(someUser1);
            post1.LikedBy.Add(someUser2);
            post1.LikedBy.Add(someUser3);
            var comment = new Comment() { Author = admin, Content = "This is the first post in this site" };
            comment.LikedBy.Add(someUser1);
            comment.LikedBy.Add(someUser2);
            comment.LikedBy.Add(someUser3);
            post1.Comments.Add(comment);

            var post2 = new Publication() { Title = "Funny story", Author = admin, ContentType = ContentType.Text, Content = Encoding.Default.GetBytes("bla bla bla bla"), PublicationDate = DateTime.Now.AddDays(-1) };
            post2.DislikedBy.Add(someUser1);
            post2.LikedBy.Add(someUser2);
            post2.DislikedBy.Add(someUser3);
            comment = new Comment() { Author = someUser1, Content = "I think its not funny" };
            comment.DislikedBy.Add(admin);
            comment.DislikedBy.Add(someUser2);
            comment.LikedBy.Add(someUser3);
            post2.Comments.Add(comment);
            comment = new Comment() { Author = someUser2, Content = "Great story!" };
            post2.Comments.Add(comment);

            memStream = new MemoryStream();
            Resources.Skyrim.Save(memStream, ImageFormat.Jpeg);
            var post3 = new Publication() { Title = "Great game, great art", Author = someUser1, ContentType = ContentType.Image, Content = memStream.ToArray(), PublicationDate = DateTime.Now };
            var post4 = new Publication() { Title = "I want just say something", Author = someUser2, ContentType = ContentType.Text, Content = Encoding.Default.GetBytes("Nothing"), PublicationDate = DateTime.Now };
            post4.DislikedBy.Add(someUser1);
            post4.DislikedBy.Add(admin);
            post4.DislikedBy.Add(someUser3);

            context.Publications.AddRange(new Publication[] { post1, post2, post3, post4 });
            context.SaveChanges();
        }
    }
}
