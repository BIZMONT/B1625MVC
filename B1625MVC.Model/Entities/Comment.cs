using System;
using System.Collections.Generic;

namespace B1625MVC.Model.Entities
{
    /// <summary>
    /// Class represents model of comment in the publication
    /// </summary>
    public class Comment
    {
        public long CommentId { get; set; }
        public string Content { get; set; }
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
        public long PublicationId { get; set; }
        public virtual Publication Publication { get; set; }

        public virtual ICollection<UserAccount> LikedBy { get; set; } = new List<UserAccount>();
        public virtual ICollection<UserAccount> DislikedBy { get; set; } = new List<UserAccount>();
    }
}
