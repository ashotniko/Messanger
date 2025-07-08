using Messanger.Models;

namespace Messanger.Interfaces
{
    public interface IMessageHelper
    {
        Task<Message?> GetMessageById(int messageId);
        Task<int?> GetSenderIdByMessageId(int messageId);
        Task<int?> GetReceiverIdByMessageId(int messageId);
    }
}
