using B1625MVC.BLL.DTO.Enums;

namespace B1625MVC.BLL.DTO
{
    public class CreatePublicationData
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public byte[] Content { get; set; }
        public ContentType ContentType { get; set; }
    }
}
