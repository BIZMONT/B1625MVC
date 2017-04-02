using System;
using System.Collections.Generic;

using B1625MVC.Model.Enums;

namespace B1625MVC.Model.Entities
{
    /// <summary>
    /// Class represents publication
    /// </summary>
    public class Publication
    {
        public Publication()
        {
            PublicationDate = DateTime.Now;
        }

        public long PublicationId { get; set; }
        public string Title { get; set; }
        public byte[] Content { get; set; }
        public ContentType ContentType { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int Rating { get; private set; }

        public string AuthorId { get; set; }
        public virtual UserProfile Author { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public virtual ICollection<UserProfile> LikedBy { get; set; } = new List<UserProfile>();
        public virtual ICollection<UserProfile> DislikedBy { get; set; } = new List<UserProfile>();
    }
}
