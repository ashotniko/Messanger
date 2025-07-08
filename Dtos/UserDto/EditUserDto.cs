using System.ComponentModel.DataAnnotations;

namespace Messanger.Dtos.UserDto
{
    public class EditUserDto
    {
        [MinLength(1, ErrorMessage = "FirstName must have minimum 1 characters.")]
        [MaxLength(50, ErrorMessage = "FirstName must have maximum 50 characters.")]
        public string? FirstName { get; set; }

        [MinLength(1, ErrorMessage = "LastName must have minimum 1 characters.")]
        [MaxLength(50, ErrorMessage = "LastName must have maximum 50 characters.")]
        public string? LastName { get; set; }

        [Range(18, 100, ErrorMessage = "Age must be between 18 - 100.")]
        public int? Age { get; set; }

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName}, Age: {this.Age}";
        }
    }
}
