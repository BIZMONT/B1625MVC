using B1625MVC.BLL.DTO.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace B1625MVC.Web.Areas.Admin.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "New password confirm")]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string NewPasswordConfirm { get; set; }

        public byte[] Avatar { get; set; }
        public Gender Gender { get; set; }

        public List<RoleCheckModel> Roles { get; set; }
    }
}