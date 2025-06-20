using Messanger.Enums;
using Microsoft.AspNetCore.Identity;

namespace Messanger.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public UserActivity Activity { get; set; } = UserActivity.Online;
        public DateTime LastSeen { get; set; } // TODO: Use LastSeen only if Activity is Offline

        public ICollection<UserGroup>? UserGroups { get; set; }
        public ICollection<Message>? ReceivedMessages { get; set; }
        public ICollection<Message>? SentMessages { get; set; }
    }
}
