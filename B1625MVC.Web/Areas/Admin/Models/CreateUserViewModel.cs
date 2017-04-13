using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace B1625MVC.Web.Areas.Admin.Models
{
    public class CreateUserViewModel
    {
        public CreateUserViewModel()
        {
            Roles = new List<RoleCheckModel>();
        }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        public List<RoleCheckModel> Roles { get; set; }
    }
}