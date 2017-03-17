using B1625MVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B1625MVC.Web.Models
{
    public class UserProfile
    {
        public UserAccount Account { get; set; }
        public UserDetails Details { get; set; }
    }
}