using B1625MVC.BLL.DTO.Enums;
using System;

namespace B1625MVC.BLL.DTO.ContentData.PublicationData
{
    public class PublicationFilters
    {
        public long Id { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
        public ContentType ContentType { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Title { get; set; }
        public int Comments { get; set; }
    }
}
