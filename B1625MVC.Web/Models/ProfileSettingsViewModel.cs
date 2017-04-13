using B1625MVC.BLL.DTO.Enums;
using System.ComponentModel.DataAnnotations;

namespace B1625MVC.Web.Models
{
    public class ProfileSettingsViewModel
    {
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }

        [Compare("NewPasswordConfirm")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        public Gender Gender { get; set; }
    }
}