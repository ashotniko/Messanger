using Messanger.Dtos.MessageDto.SingleUser;
using Messanger.Models;

namespace Messanger.Interfaces
{
    public interface IMessageService
    {
        public Task<IEnumerable<GetMessageForSingelUserDto>> GetAllMessages();
        public Task<GetMessageForSingelUserDto> GetMessageById(int messageId);
        public Task EditMessage(int id, EditMessageForSingleUserDto editMessageDto);
        public Task<Message> CreateMessageAsync(int senderId, int receiverId, CreateMessageForSingleUserDto createMessageDto);
        public Task DeleteMessage(int id);
    }
}
