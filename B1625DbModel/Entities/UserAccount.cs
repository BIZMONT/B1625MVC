using System.Collections.Generic;

namespace B1625DbModel.Entities
{
    public class UserAccount
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Publication> Publications { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual UserDetails Details { get; set; }
    }
}
