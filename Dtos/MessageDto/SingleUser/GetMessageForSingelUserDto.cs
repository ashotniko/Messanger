using Messanger.Dtos.UserDto;
using Messanger.Enums;

namespace Messanger.Dtos.MessageDto.SingleUser
{
    public class GetMessageForSingelUserDto
    {
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public MessageState State { get; set; }
        public GetUserDto Sender { get; set; }
        public GetUserDto Receiver { get; set; }
    }
}
