using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B1625MVC.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [Display(Name ="Username or email")]
        [MaxLength(32)]
        [MinLength(5)]
        public string EmailOrUserName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimum password length is 8 symbols")]
        [MaxLength(32, ErrorMessage = "Maximum password length is 32 symbols")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name ="Remember me")]
        public bool RememberMe { get; set; }
    }
}