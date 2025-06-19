using Messanger.Models;

namespace Messanger.Interfaces
{
    public interface IMessageHelper
    {
        public Task<IEnumerable<Message>> GetAllMessages();
        public Task<Message> GetMessageById(int messageId);
    }
}
