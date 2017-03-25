using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace B1625MVC.Model.Entities
{
    public class UserAccount : IdentityUser
    {
        public virtual UserProfile Profile { get; set; }
    }
}
