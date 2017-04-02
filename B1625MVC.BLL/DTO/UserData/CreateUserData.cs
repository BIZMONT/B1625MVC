using B1625MVC.Model.Enums;
using System.Collections.Generic;

namespace B1625MVC.BLL.DTO
{
    public class CreateUserData : BaseUserData
    {
        public string NewPassword { get; set; }
    }
}
