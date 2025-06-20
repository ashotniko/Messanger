using Messanger.Enums;

namespace Messanger.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public MessageState State { get; set; }
        public int SenderId { get; set; }
        public ApplicationUser Sender { get; set; } = null!;
        public int ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; } = null!;
        public int? GroupId { get; set; }
        public Group? Group { get; set; }
    }
}
