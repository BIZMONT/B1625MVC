using System;
using System.Collections.Generic;

namespace B1625MVC.Model.Entities
{
    /// <summary>
    /// Class represents model of comment in the publication
    /// </summary>
    public class Comment
    {
        public Comment()
        {
            PublicationDate = DateTime.Now;
        }

        public long CommentId { get; set; }
        public string Content { get; set; }
        public DateTime? PublicationDate { get; set; }

        public int Rating
        {
            get
            {
                return LikedBy.Count - DislikedBy.Count;
            }
            private set { }
        }

        public string AuthorId { get; set; }
        public virtual UserProfile Author { get; set; }
        public long PublicationId { get; set; }
        public virtual Publication Publication { get; set; }

        public virtual ICollection<UserProfile> LikedBy { get; set; } = new List<UserProfile>();
        public virtual ICollection<UserProfile> DislikedBy { get; set; } = new List<UserProfile>();
    }
}
