using Messanger.Enums;

namespace Messanger.Dtos.UserDto
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public UserActivity Activity { get; set; }
        public DateTime LastSeen { get; set; } // TODO: Use LastSeen only if Activity is Offline

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName}, Age: {this.Age}";
        }
    }
}
