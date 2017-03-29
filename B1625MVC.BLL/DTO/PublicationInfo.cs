namespace B1625MVC.BLL.DTO
{
    public class PublicationInfo : ContentInfo
    {
        public string Title { get; set; }
        public ContentType ContentType { get; set; }
        public byte[] Content { get; set; }
        public int CommentsCount { get; set; }
    }
}
