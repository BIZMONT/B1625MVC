using System.ComponentModel.DataAnnotations;

namespace B1625MVC.Web.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(5, ErrorMessage = "Username must be more than 5 characters")]
        [MaxLength(32)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimum password length is 8 symbols")]
        [MaxLength(32, ErrorMessage = "Maximum password length is 32 symbols")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords not mach")]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}