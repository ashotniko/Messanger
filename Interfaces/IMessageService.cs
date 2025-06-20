using Messanger.Dtos.MessageDto.SingleUser;

namespace Messanger.Interfaces
{
    public interface IMessageService
    {
        public Task<IEnumerable<GetMessageForSingelUserDto>> GetAllMessages();
        public Task<GetMessageForSingelUserDto> GetMessageById(int messageId);
        public Task EditMessage(int id, EditMessageForSingleUserDto editMessageDto);
        public Task<GetMessageForSingelUserDto> CreateMessageAsync(int senderId, int receiverId, CreateMessageForSingleUserDto createMessageDto, CancellationToken ct = default);
        public Task DeleteMessage(int id);
    }
}
