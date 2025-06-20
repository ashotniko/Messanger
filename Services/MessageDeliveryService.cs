//using Messanger.Dtos.MessageDto.SingleUser;
//using Messanger.Enums;
//using Messanger.Hubs;
//using Messanger.Interfaces;
//using Messanger.Models;
//using Microsoft.AspNetCore.SignalR;

//namespace Messanger.Services
//{
//    public class MessageDeliveryService
//    {
//        private readonly IUserService _userService;
//        private readonly IMessageService _messageService;
//        private readonly IHubContext<ChatHub> _hub;
//        private readonly IMessageHelper _messageHelper;

//        public MessageDeliveryService(IUserService userService,
//            IMessageService messageService,
//            IHubContext<ChatHub> hub,
//            IMessageHelper messageHelper)
//        {
//            _userService = userService;
//            _messageService = messageService;
//            _hub = hub;
//            _messageHelper = messageHelper;
//        }

//        public async Task<Message> SendToSingleUserAsync(
//            int senderId,
//            int receiverId,
//            CreateMessageForSingleUserDto dto,
//            CancellationToken ct = default)
//        {
//            var savedMessage = await _messageService.CreateMessageAsync(senderId, receiverId, dto, ct);
//            var message = await _messageHelper.GetMessageById(savedMessage.Id);

//            message.State = MessageState.Sent;
//            message.SentAt = DateTime.UtcNow;
//            //message.SenderId = DateTime.UtcNow; // Get user id who  logged in  and write here

//            await _hub.Clients.Group($"user-{dto.ReceiverId}")
//                .SendAsync("ReceiveMessage", message, ct);

//            return savedMessage;
//        }
//    }
//}
