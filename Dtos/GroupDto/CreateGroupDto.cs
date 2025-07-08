using Messanger.Models;
using System.ComponentModel.DataAnnotations;

namespace Messanger.Dtos.GroupDto;

public class CreateGroupDto
{
    [Required]
    public int CreatorId { get; set; }
    [Required]
    public string Name { get; set; }
    public ICollection<UserGroup>? UserGroups { get; set; }
    public ICollection<Message>? Messages { get; set; }
}
