using System.ComponentModel.DataAnnotations;

namespace Messanger.Dtos.MessageDto.SingleUser
{
    public class EditMessageForSingleUserDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Content must have minimum 1 characters.")]
        [MaxLength(500, ErrorMessage = "Content must have maximum 500 characters.")]
        public string Content { get; set; }
    }
}
