namespace B1625DbModel.Entities
{
    /// <summary>
    /// Class represents model of comment in the publication
    /// </summary>
    public class Comment
    {
        public long CommentId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }

        public long AuthorId { get; set; }
        public virtual UserAccount Author { get; set; }
        public long PublicationId { get; set; }
        public virtual Publication Publication { get; set; }
    }
}
