using Messanger.Dtos.UserDto;
using Messanger.Models;

namespace Messanger.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<GetUserDto>> GetAllUsers();
        public Task<GetUserDto> GetUser(int id);
        public Task EditUser(int id, EditUserDto editUserDto);
        public Task<User> CreateUser(CreateUserDto createUserDto);
        public Task DeleteUser(int id);
    }
}
