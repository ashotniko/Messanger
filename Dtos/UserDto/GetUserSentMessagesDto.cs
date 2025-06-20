using Messanger.Models;

namespace Messanger.Dtos.UserDto
{
    public class GetUserSentMessagesDto
    {
        public int Id { get; set; }
        public ICollection<Message>? SentMessages { get; set; }
    }
}
