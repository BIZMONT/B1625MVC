using B1625MVC.BLL.DTO.Enums;
using System.ComponentModel.DataAnnotations;

namespace B1625MVC.Web.Models
{
    public class ProfileSettingsViewModel
    {
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string NewPasswordConfirm { get; set; }

        [Compare("NewPasswordConfirm")]
        [DataType(DataType.Password)]
        [Display(Name ="New password")]
        public string NewPassword { get; set; }

        public Gender Gender { get; set; }

        public byte[] Avatar { get; set; }
    }
}