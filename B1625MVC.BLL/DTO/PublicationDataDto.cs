using B1625MVC.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1625MVC.BLL.DTO
{
    public class PublicationDataDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public ContentType ContentType { get; set; }
    }
}
