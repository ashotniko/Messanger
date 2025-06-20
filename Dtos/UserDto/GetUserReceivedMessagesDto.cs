using Messanger.Models;

namespace Messanger.Dtos.UserDto
{
    public class GetUserReceivedMessagesDto
    {
        public int Id { get; set; }
        public ICollection<Message>? ReceivedMessages { get; set; }
    }
}
