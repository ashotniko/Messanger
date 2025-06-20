using Messanger.Dtos.MessageDto.SingleUser;
using Messanger.Enums;
using Messanger.Models;

namespace Messanger.Mappers
{
    public static class MessageMapper
    {
        public static Message ToMessageFromDto(CreateMessageForSingleUserDto dto, int senderId, int receiverID)
        {
            return new Message
            {
                Content = dto.Content,
                State = MessageState.Created,
                SenderId = senderId,
                ReceiverId = receiverID,
            };
        }

        public static GetMessageForSingelUserDto FromMessageToDto(this Message message)
        {
            return new GetMessageForSingelUserDto
            {
                Id = message.Id,
                Content = message.Content,
                State = message.State,
                Sender = message.Sender.ToDtoFromUser(),
                Receiver = message.Receiver.ToDtoFromUser(),
            };
        }
    }
}
