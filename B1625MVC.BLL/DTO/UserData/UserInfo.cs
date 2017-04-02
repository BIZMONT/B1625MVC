using System;

namespace B1625MVC.BLL.DTO
{
    public class UserInfo : BaseUserData
    {
        public string Id { get; set; }
        public int Rating { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
