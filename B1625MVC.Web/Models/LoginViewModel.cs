using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace B1625MVC.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name ="Username or email")]
        public string EmailOrUserName { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [Display(Name ="Remember me")]
        public bool RememberMe { get; set; }
    }
}