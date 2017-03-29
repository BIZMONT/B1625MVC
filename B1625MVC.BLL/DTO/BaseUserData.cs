using B1625MVC.Model.Enums;
using System.Collections.Generic;

namespace B1625MVC.BLL.DTO
{
    public abstract class BaseUserData
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public byte[] Avatar { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
