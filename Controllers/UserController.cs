using Messanger.Dtos.UserDto;
using Messanger.Helpers;
using Messanger.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messanger.Controllers
{
    [Authorize(Policy = "UserPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UserController> logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.logger = logger;
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                var users = await this.userService.GetAllUsers();
                this.logger.LogInformation($"Retrieved {users.Count()} users from the database.");
                return this.Ok(users);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while retrieving users.");
                return this.StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetUserById([FromRoute] int id)
        {
            try
            {
                var user = await this.userService.GetUser(id);
                this.logger.LogInformation($"Retrieved user with id {id} from the database.");
                return this.Ok(user);
            }
            catch (ArgumentNullException ex)
            {
                this.logger.LogWarning(ex.Message);
                return this.NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var user = await this.userService.CreateUser(createUserDto);
                this.logger.LogInformation($"Created user with id {user.Id}.");
                return this.CreatedAtAction(
                    nameof(GetUserById),
                    new { id = user.Id },
                    user
                );
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
        public async Task<IActionResult> EditUser([FromRoute] int id, [FromBody] EditUserDto editUserDto)
        {
            try
            {
                if (UserIdentifierHelper.IsSelfOrAdmin(this.User, id))
                {
                    await this.userService.EditUser(id, editUserDto);
                    this.logger.LogInformation($"Edited user with id {id}.");
                    return this.NoContent();
                }

                this.logger.LogWarning($"You cannot edit user with id {id} without permission.");
                return this.Forbid();

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
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                if (UserIdentifierHelper.IsSelfOrAdmin(this.User, id))
                {
                    await this.userService.DeleteUser(id);
                    this.logger.LogInformation($"Deleted user with id {id}.");
                    return this.NoContent();
                }
                this.logger.LogWarning($"You cannot delete user with id {id} without permission.");
                return this.Forbid();

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
