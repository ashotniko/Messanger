namespace Messanger.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<UserGroup>? UserGroups { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
