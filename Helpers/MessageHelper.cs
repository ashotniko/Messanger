using Messanger.Data;
using Messanger.Dtos.UserDto;
using Messanger.Interfaces;
using Messanger.Mappers;
using Messanger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Helpers
{
    public class MessageHelper : IMessageHelper
    {
        private readonly MessengerDbContext _context;

        public MessageHelper(MessengerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all messages in the system.
        /// </summary>
        /// <returns>A list of <see cref="Message"/>.</returns>
        public async Task<IEnumerable<Message>> GetAllMessages()
        {
            return await _context.Messages.ToListAsync();
        }

        /// <summary>
        /// Retrieves a message by its unique ID.
        /// </summary>
        /// <param name="messageId">The ID of the message to retrieve.</param>
        /// <returns>The corresponding <see cref="Message"/>.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no message with the specified ID is found.</exception>
        public async Task<Message> GetMessageById(int messageId)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
            if (message == null) throw new KeyNotFoundException($"Message with id {messageId} not found.");

            return message;
        }

        public async Task<GetUserSentMessagesDto> GetUsersSentMessages(int userId)
        {
            var user = await _context.Users
                .Include(u => u.SentMessages)
                .FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new KeyNotFoundException($"User {userId} not found.");

            return user.ToGetUserSentMessagesDtoFromUser();
        }

        public async Task<GetUserReceivedMessagesDto> GetUsersReceivedMessages(int userId)
        {
            var user = await _context.Users
                .Include(u => u.SentMessages)
                .FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new KeyNotFoundException($"User {userId} not found.");

            return user.ToGetUserReceivedMessagesDtoFromUser();
        }
    }
}
