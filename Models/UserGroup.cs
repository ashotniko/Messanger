namespace Messanger.Models
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
