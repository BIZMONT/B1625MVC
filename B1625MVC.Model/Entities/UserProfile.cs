using B1625MVC.Model.Enums;
using System;
using System.Collections.Generic;

namespace B1625MVC.Model.Entities
{
    public class UserProfile
    {
        public string AccountId { get; set; }

        public byte[] Avatar { get; set; }
        public Gender Gender { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public int Rating
        {
            get
            {
                int rating = 0;
                foreach (Publication publication in Publications)
                {
                    rating += publication.Rating;
                }
                foreach (Comment comment in Comments)
                {
                    rating += comment.Rating;
                }
                return rating;
            }
            private set { }
        }
        public string UserName
        {
            get
            {
                return User.UserName;
            }
        }

        public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
        public virtual ICollection<Publication> LikedPublications { get; set; } = new List<Publication>();
        public virtual ICollection<Publication> DislikedPublications { get; set; } = new List<Publication>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Comment> LikedComments { get; set; } = new List<Comment>();
        public virtual ICollection<Comment> DislikedComments { get; set; } = new List<Comment>();

        public virtual UserAccount User { get; set; }
    }
}
