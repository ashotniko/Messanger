using Messanger.Dtos.AccountDto;
using Messanger.Enums;
using Messanger.Interfaces;
using Messanger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Messanger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService tokenService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger logger;

        public AccountController(
            ITokenService tokenService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            this.tokenService = tokenService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto dto)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = dto.UserName,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age,
                    Email = dto.Email
                };

                var createdUser = await this.userManager.CreateAsync(user, dto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await this.userManager.AddToRoleAsync(user, "User");

                    if (roleResult.Succeeded)
                    {
                        return this.Ok(
                            new NewUserDto
                            {
                                UserName = dto.UserName,
                                FirstName = dto.FirstName,
                                LastName = dto.LastName,
                                Age = dto.Age,
                                Activity = UserActivity.Online,
                                LastSeen = DateTime.UtcNow,
                                Email = dto.Email,
                                Token = await this.tokenService.CreateTokenAsync(user)
                            }
                        );
                    }
                    else
                    {
                        this.logger.LogError(roleResult.Errors.ToString());
                        return this.BadRequest(roleResult.Errors);
                    }
                }
                else
                {
                    this.logger.LogError(createdUser.Errors.ToString());
                    return this.BadRequest(createdUser.Errors);
                }

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return this.BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            try
            {
                var user = this.userManager.Users.FirstOrDefault(u => u.UserName.ToLower() == loginDto.UserName.ToLower());
                if (user == null)
                {
                    this.logger.LogError($"User with username {loginDto.UserName} doesn't exists.");
                    return this.Unauthorized("Invalid Username");
                }
                var result = await this.signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                ;

                if (!result.Succeeded)
                {
                    this.logger.LogError("Password isnt correct");
                    return this.Unauthorized("Password isnt correct");
                }

                return this.Ok(new NewUserDto()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    Activity = UserActivity.Online,
                    LastSeen = DateTime.UtcNow,
                    Email = user.Email,
                    Token = await this.tokenService.CreateTokenAsync(user)
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return this.BadRequest(ex.Message);
            }
        }
    }
}
