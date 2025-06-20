using Messanger.Enums;

namespace Messanger.Dtos.AccountDto
{
    public class NewUserDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public UserActivity Activity { get; set; } = UserActivity.Online;
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
