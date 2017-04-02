using B1625MVC.BLL.DTO.Enums;

namespace B1625MVC.Web.Models
{
    public class ProfileSettingsViewModel
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string OldPasswordConfirm { get; set; }
        public string NewPassword { get; set; }
        public Gender Gender { get; set; }
    }
}