using Messanger.Dtos.UserDto;

namespace Messanger.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<GetUserDto>> GetAllUsers();
        Task<GetUserDto> GetUser(int id);
        Task EditUser(int id, EditUserDto editUserDto);
        Task<GetUserDto> CreateUser(CreateUserDto createUserDto);
        Task DeleteUser(int id);
    }
}
