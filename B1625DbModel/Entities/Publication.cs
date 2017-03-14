using System.Collections.Generic;

namespace B1625DbModel.Entities
{
    /// <summary>
    /// Class represents publication
    /// </summary>
    public class Publication
    {
        public long PublicationId { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public byte[] Content { get; set; }
        public ContentType ContentType { get; set; }

        public long AuthorId { get; set; }
        public virtual UserAccount Author { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } 
    }
}
