using Messanger.Enums;

namespace Messanger.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public UserActivity Activity { get; set; } = UserActivity.Online;
        public DateTime LastSeen { get; set; }

        public ICollection<UserGroup>? UserGroups { get; set; }
        public ICollection<Message>? ReceivedMessages { get; set; }
        public ICollection<Message>? SentMessages { get; set; }
    }
}
