using Messanger.Dtos.MessageDto.SingleUser;
using Messanger.Enums;
using Messanger.Models;

namespace Messanger.Mappers
{
    public static class MessageMapper
    {
        public static Message ToMessageFromDto(CreateMessageForSingleUserDto dto)
        {
            return new Message
            {
                Content = dto.Content,
                State = MessageState.Created,
            };
        }

        public static GetMessageForSingelUserDto FromMessageToDto(this Message message)
        {
            return new GetMessageForSingelUserDto
            {
                Content = message.Content,
                State = message.State,
                Sender = message.Sender.GetUserDto(),
                Receiver = message.Receiver.GetUserDto()
            };
        }
    }
}
