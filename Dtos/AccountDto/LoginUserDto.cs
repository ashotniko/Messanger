using System.ComponentModel.DataAnnotations;

namespace Messanger.Dtos.AccountDto
{
    public class LoginUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
