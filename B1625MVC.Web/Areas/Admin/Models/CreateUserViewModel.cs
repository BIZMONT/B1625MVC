using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B1625MVC.Web.Areas.Admin.Models
{
    public class CreateUserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}