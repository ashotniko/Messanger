using Messanger.Dtos.UserDto;

namespace Messanger.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<GetUserDto>> GetAllUsers();
        public Task<GetUserDto> GetUser(int id);
        public Task EditUser(int id, EditUserDto editUserDto);
        public Task<GetUserDto> CreateUser(CreateUserDto createUserDto);
        public Task DeleteUser(int id);
    }
}
