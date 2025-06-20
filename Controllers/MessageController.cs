using Messanger.Dtos.MessageDto.SingleUser;
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
        private readonly IMessageService _messageService;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMessageService messageService, ILogger<MessageController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<ActionResult> GetAllMessages()
        {
            try
            {
                var messages = await _messageService.GetAllMessages();
                _logger.LogInformation($"Retrieved {messages.Count()} messages from the database.");
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving messages.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetMessageById([FromRoute] int id)
        {
            try
            {
                var message = await _messageService.GetMessageById(id);
                _logger.LogInformation($"Retrieved message with id {id} from the database.");
                return Ok(message);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
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
                var message = await _messageService.CreateMessageAsync(senderId, receiverId, createMessageDto, ct);
                _logger.LogInformation($"Created message with id {message.Id}.");
                return CreatedAtAction(
                    nameof(GetMessageById),
                    new { id = message.Id },
                    message
                );
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditMessage(int id, [FromBody] EditMessageForSingleUserDto editMessageDto)
        {
            try
            {
                await _messageService.EditMessage(id, editMessageDto);
                _logger.LogInformation($"Edited message with id {id}.");
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                await _messageService.DeleteMessage(id);
                _logger.LogInformation($"Deleted message with id {id}.");
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
