using System;
using System.Collections.Generic;

using B1625MVC.Model.Infrastructure;

namespace B1625MVC.Model.Entities
{
    /// <summary>
    /// Class represents publication
    /// </summary>
    public class Publication
    {
        public long PublicationId { get; set; }
        public string Title { get; set; }
        public byte[] Content { get; set; }
        public ContentType ContentType { get; set; }
        public DateTime? PublicationDate { get; set; }

        public int Rating
        {
            get
            {
                return LikedBy.Count - DislikedBy.Count;
            }
        }

        public long AuthorId { get; set; }
        public virtual UserAccount Author { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public virtual ICollection<UserAccount> LikedBy { get; set; } = new List<UserAccount>();
        public virtual ICollection<UserAccount> DislikedBy { get; set; } = new List<UserAccount>();
    }
}
