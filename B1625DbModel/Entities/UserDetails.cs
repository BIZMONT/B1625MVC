namespace B1625DbModel.Entities
{
    public class UserDetails
    {
        public long UserId { get; set; }

        public byte[] Avatar { get; set; }
        public Gender Gender { get; set; }

        public virtual UserAccount User { get; set; }
    }
}
