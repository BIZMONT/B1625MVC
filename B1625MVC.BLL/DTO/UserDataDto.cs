using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B1625MVC.Model.Enums;

namespace B1625MVC.BLL.DTO
{
    public class UserDataDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public byte[] Avatar { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public int Rating { get; set; }
    }
}
