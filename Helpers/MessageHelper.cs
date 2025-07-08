using Messanger.Data;
using Messanger.Interfaces;
using Messanger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Helpers
{
    public class MessageHelper : IMessageHelper
    {
        private readonly MessengerDbContext context;

        public MessageHelper(MessengerDbContext context)
        {
            this.context = context;
        }

        public async Task<Message?> GetMessageById(int messageId)
        {
            return await this.context.Messages
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<int?> GetSenderIdByMessageId(int messageId)
        {
            return await this.context.Messages
                             .Where(m => m.Id == messageId)
                             .AsNoTracking()
                             .Select(m => (int?)m.SenderId)
                             .FirstOrDefaultAsync();
        }

        public async Task<int?> GetReceiverIdByMessageId(int messageId)
        {
            return await this.context.Messages
                             .Where(m => m.Id == messageId)
                             .AsNoTracking()
                             .Select(m => (int?)m.ReceiverId)
                             .FirstOrDefaultAsync();
        }
    }
}
