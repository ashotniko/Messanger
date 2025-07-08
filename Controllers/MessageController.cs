using Messanger.Data;
using Messanger.Dtos.MessageDto.SingleUser;
using Messanger.Helpers;
using Messanger.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messanger.Controllers
{
    [Authorize(Policy = "UserPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService messageService;
        private readonly IMessageHelper messageHelper;
        private readonly MessengerDbContext context;
        private readonly ILogger<MessageController> logger;

        public MessageController(
            IMessageService messageService,
            IMessageHelper messageHelper,
            MessengerDbContext context,
            ILogger<MessageController> logger
            )
        {
            this.messageService = messageService;
            this.messageHelper = messageHelper;
            this.context = context;
            this.logger = logger;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<ActionResult> GetAllMessages()
        {
            try
            {
                var messages = await this.messageService.GetAllMessages();
                this.logger.LogInformation($"Retrieved {messages.Count()} messages from the database.");
                return this.Ok(messages);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while retrieving messages.");
                return this.StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetMessageById([FromRoute] int id)
        {
            try
            {
                var message = await this.messageService.GetMessageById(id);
                var senderId = await this.messageHelper.GetSenderIdByMessageId(id);
                var receiverId = await this.messageHelper.GetReceiverIdByMessageId(id);

                if (UserIdentifierHelper.IsSenderReceiverOrAdmin(this.User, senderId, receiverId))
                {
                    this.logger.LogInformation($"Retrieved message with id {id} from the database.");
                    return this.Ok(message);
                }

                this.logger.LogWarning($"You cannot view messages for user with id {id} without permission.");
                return this.Forbid();
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogWarning(ex.Message);
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult> GetMessagesByUserId([FromRoute] int userId)
        {
            try
            {
                if (UserIdentifierHelper.IsSelfOrAdmin(this.User, userId))
                {
                    var messages = await this.messageService.GetAllMessagesForUser(userId);
                    if (messages.Count() == 0)
                    {
                        this.logger.LogInformation($"No messages found for user with id {userId}.");
                        return this.NotFound($"No messages found for user with id {userId}.");
                    }
                    this.logger.LogInformation($"Retrieved {messages.Count()} messages for user with id {userId}.");
                    return this.Ok(messages);
                }
                else
                {
                    this.logger.LogWarning($"You cannot view messages for user with id {userId} without permission.");
                    return this.Forbid();
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return this.NotFound(ex.Message);
            }
        }

        [HttpPost("{senderId:int}/{receiverId:int}")]
        public async Task<IActionResult> CreateMessage(
            [FromRoute] int senderId,
            [FromRoute] int receiverId,
            [FromBody] CreateMessageForSingleUserDto createMessageDto,
            CancellationToken ct = default)
        {
            try
            {
                if (UserIdentifierHelper.IsSelfOrAdmin(this.User, senderId))
                {
                    var message = await this.messageService.CreateMessageAsync(senderId, receiverId, createMessageDto, ct);
                    this.logger.LogInformation($"Created message with id {message.Id}.");
                    return this.CreatedAtAction(
                        nameof(GetMessageById),
                        new { id = message.Id },
                        message
                    );
                }
                else
                {
                    this.logger.LogWarning($"You cannot create messages for user with id {senderId} without permission.");
                    return this.Forbid();
                }
            }
            catch (ArgumentException ex)
            {
                this.logger.LogWarning(ex.Message);
                return this.Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditMessage(int id, [FromBody] EditMessageForSingleUserDto editMessageDto)
        {
            try
            {
                var senderId = await this.messageHelper.GetSenderIdByMessageId(id);

                if (UserIdentifierHelper.IsSelfOrAdmin(this.User, senderId))
                {
                    await this.messageService.EditMessage(id, editMessageDto);
                    this.logger.LogInformation($"Edited message with id {id}.");
                    return this.NoContent();
                }
                else
                {
                    this.logger.LogWarning($"You cannot edit messages for user with id {senderId} without permission.");
                    return this.Forbid();
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogWarning(ex.Message);
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return this.BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                var senderId = await this.messageHelper.GetSenderIdByMessageId(id);
                var receiverId = await this.messageHelper.GetReceiverIdByMessageId(id);

                if (UserIdentifierHelper.IsSenderReceiverOrAdmin(this.User, senderId, receiverId))
                {
                    await this.messageService.DeleteMessage(id);
                    this.logger.LogInformation($"Deleted message with id {id}.");
                    return this.NoContent();
                }
                else
                {
                    this.logger.LogWarning($"You cannot delete messages without permission.");
                    return this.Forbid();
                }
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogWarning(ex.Message);
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return this.BadRequest(ex.Message);
            }
        }
    }
}
