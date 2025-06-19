using Messanger.Data;
using Messanger.Dtos.MessageDto.SingleUser;
using Messanger.Interfaces;
using Messanger.Mappers;
using Messanger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Services
{
    public class MessageService : IMessageService
    {
        private readonly MessengerDbContext _context;

        public MessageService(MessengerDbContext context)
        {
            _context = context;
        }

        // ─────────────────────────────────────────────────────────────────────────
        // CREATE
        // ─────────────────────────────────────────────────────────────────────────
        public async Task<Message> CreateMessageAsync(
            int senderId,
            int receiverId,
            CreateMessageForSingleUserDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto), "Create message DTO cannot be null.");

            var users = await _context.Users
                .Where(u => u.Id == senderId || u.Id == receiverId)
                .ToListAsync();

            var sender = users.SingleOrDefault(u => u.Id == senderId);
            if (sender == null) throw new KeyNotFoundException($"Sender with id {senderId} not found.");

            var receiver = users.SingleOrDefault(u => u.Id == receiverId);
            if (receiver == null) throw new KeyNotFoundException($"Receiver with id {receiverId} not found.");

            var message = MessageMapper.ToMessageFromDto(dto);
            message.SenderId = senderId;
            message.ReceiverId = receiverId;

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return message;
        }

        /// <summary>
        /// Deletes a message by its ID.
        /// </summary>
        /// <param name="id">The ID of the message to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the message does not exist.</exception>
        public async Task DeleteMessage(int id)
        {
            var message = _context.Messages.FirstOrDefault(m => m.Id == id);
            if (message == null) throw new KeyNotFoundException($"Message with id {id} not found.");

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Edits the content of a message.
        /// </summary>
        /// <param name="id">The ID of the message to edit.</param>
        /// <param name="editMessageDto">The DTO containing updated content.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="editMessageDto"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the message does not exist.</exception>
        public async Task EditMessage(int id, EditMessageForSingleUserDto editMessageDto)
        {
            if (editMessageDto == null) throw new ArgumentNullException(nameof(editMessageDto), "Edit message DTO cannot be null.");
            var message = _context.Messages.FirstOrDefault(m => m.Id == id);

            if (message == null) throw new KeyNotFoundException($"Message with id {id} not found.");
            EditMessageHelper(message, editMessageDto);

            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all messages in the system.
        /// </summary>
        /// <returns>A list of <see cref="GetMessageForSingelUserDto"/>.</returns>
        public async Task<IEnumerable<GetMessageForSingelUserDto>> GetAllMessages()
        {
            var messages = _context.Messages
                .Select(m => m.FromMessageToDto());

            return await messages.ToListAsync();
        }

        /// <summary>
        /// Retrieves a message by its unique ID.
        /// </summary>
        /// <param name="messageId">The ID of the message to retrieve.</param>
        /// <returns>The corresponding <see cref="GetMessageForSingelUserDto"/>.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no message with the specified ID is found.</exception>
        public async Task<GetMessageForSingelUserDto> GetMessageById(int messageId)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
            if (message == null) throw new KeyNotFoundException($"Message with id {messageId} not found.");

            var sender = await _context.Users.FirstOrDefaultAsync(s => s.Id == message.SenderId);
            if (sender == null) throw new KeyNotFoundException($"Sender with id {message.SenderId} not found.");

            var receiver = await _context.Users.FirstOrDefaultAsync(r => r.Id == message.ReceiverId);
            if (receiver == null) throw new KeyNotFoundException($"Receiver with id {message.ReceiverId} not found.");

            message.Sender = sender;
            message.Receiver = receiver;

            return message.FromMessageToDto();
        }

        /// <summary>
        /// Updates the content of a message using values from the DTO.
        /// </summary>
        /// <param name="message">The existing message entity.</param>
        /// <param name="editMessageDto">The DTO with the updated content.</param>
        private void EditMessageHelper(Message message, EditMessageForSingleUserDto editMessageDto)
        {
            message.Content = editMessageDto.Content;
        }
    }
}
