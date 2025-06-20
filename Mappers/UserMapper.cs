using Messanger.Dtos.UserDto;
using Messanger.Enums;
using Messanger.Models;

namespace Messanger.Mappers
{
    public static class UserMapper
    {
        public static ApplicationUser ToUserFromCreteUserDto(CreateUserDto createUserDto)
        {
            return new ApplicationUser
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Age = createUserDto.Age,
                Activity = UserActivity.Online,
                LastSeen = DateTime.UtcNow // Default to current time for online users
            };
        }

        public static GetUserDto ToDtoFromUser(this ApplicationUser user)
        {
            return new GetUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                Activity = user.Activity,
                LastSeen = user.LastSeen
            };
        }

        public static GetUserSentMessagesDto ToGetUserSentMessagesDtoFromUser(this ApplicationUser user)
        {
            return new GetUserSentMessagesDto
            {
                Id = user.Id,
                SentMessages = user.SentMessages ?? new List<Message>()
            };
        }

        public static GetUserReceivedMessagesDto ToGetUserReceivedMessagesDtoFromUser(this ApplicationUser user)
        {
            return new GetUserReceivedMessagesDto
            {
                Id = user.Id,
                ReceivedMessages = user.ReceivedMessages ?? new List<Message>()
            };
        }
    }
}
