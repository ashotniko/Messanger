using Messanger.Data;
using Messanger.Dtos.UserDto;
using Messanger.Interfaces;
using Messanger.Mappers;
using Messanger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Services
{
    public class UserService : IUserService
    {
        private readonly MessengerDbContext _context;

        public UserService(MessengerDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Add new user to database
        /// </summary>
        /// <param name="createUserDto">User class with only nessesary parameters for creating</param>
        /// <returns>Returns creted user class</returns>
        public async Task<GetUserDto> CreateUser(CreateUserDto createUserDto)
        {
            if (createUserDto == null)
            {
                throw new ArgumentNullException(nameof(createUserDto), "CreateUserDto cannot be null.");
            }

            var newUser = UserMapper.ToUserFromCreteUserDto(createUserDto);

            await this._context.Users.AddAsync(newUser);
            await this._context.SaveChangesAsync();

            return newUser.ToDtoFromUser();
        }

        /// <summary>
        /// Delete user from database by id
        /// </summary>
        /// <param name="id">Unical identifier of user</param>
        public async Task DeleteUser(int id)
        {
            var user = this._context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }

            this._context.Users.Remove(user);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Edit user parameterts in DB
        /// </summary>
        /// <param name="id">Unical identifier of user</param>
        /// <param name="editUserDto">Helper class where is only nessesary parameters for user</param>
        public async Task EditUser(int id, EditUserDto editUserDto)
        {
            if (editUserDto == null)
            {
                throw new ArgumentNullException(nameof(editUserDto), "EditUserDto cannot be null.");
            }

            var user = this._context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }

            // Update user properties from editUserDto
            EditUserHelper(user, editUserDto);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Get all users from database
        /// </summary>
        /// <returns>Return list of User's</returns>
        public async Task<IEnumerable<GetUserDto>> GetAllUsers()
        {
            var users = this._context.Users
                .Select(u => u.ToDtoFromUser());

            return await users.ToListAsync();
        }

        /// <summary>
        /// Get user by id from database
        /// </summary>
        /// <param name="id">Unical identifier of user</param>
        /// <returns>Return user</returns>
        public async Task<GetUserDto> GetUser(int id)
        {
            var user = await this._context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found.");
            }

            return user.ToDtoFromUser();
        }

        private static void EditUserHelper(ApplicationUser user, EditUserDto editUserDto)
        {
            if (!string.IsNullOrWhiteSpace(editUserDto.FirstName))
            {
                user.FirstName = editUserDto.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(editUserDto.LastName))
            {
                user.LastName = editUserDto.LastName;
            }

            if (editUserDto.Age.HasValue)
            {
                user.Age = editUserDto.Age.Value;
            }
        }
    }
}
