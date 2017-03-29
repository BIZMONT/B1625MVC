using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1625MVC.BLL.DTO
{
    public abstract class ContentInfo
    {
        public long Id { get; set; }
        public string Author { get; set; }
        public byte[] AuthorAvatar { get; set; }
        public int Rating { get; set; }
        public RateState RateState { get; set; }
        public DateTime? PublicationDate { get; set; }
    }
}
