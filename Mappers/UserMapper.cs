using Messanger.Dtos.UserDto;
using Messanger.Enums;
using Messanger.Models;

namespace Messanger.Mappers
{
    public static class UserMapper
    {
        public static User ToUserFromCreteUserDto(CreateUserDto createUserDto)
        {
            return new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Age = createUserDto.Age,
                Activity = UserActivity.Online,
                LastSeen = DateTime.UtcNow // Default to current time for online users
            };
        }

        public static GetUserDto GetUserDto(this User user)
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
    }
}
