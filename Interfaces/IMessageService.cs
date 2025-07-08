using Messanger.Dtos.MessageDto.SingleUser;

namespace Messanger.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<GetMessageForSingelUserDto>> GetAllMessages();
        Task<GetMessageForSingelUserDto> GetMessageById(int messageId);
        Task<IEnumerable<GetMessageForSingelUserDto>> GetAllMessagesForUser(int userId);
        Task EditMessage(int id, EditMessageForSingleUserDto editMessageDto);
        Task<GetMessageForSingelUserDto> CreateMessageAsync(int senderId, int receiverId, CreateMessageForSingleUserDto createMessageDto, CancellationToken ct = default);
        Task DeleteMessage(int id);
    }
}
