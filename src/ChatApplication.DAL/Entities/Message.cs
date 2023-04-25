using ChatApplication.DAL.Entities.Interfaces;

namespace ChatApplication.DAL.Entities;

public class Message: IAuditableEntity
{
    public int Id { get; set; }
    public int? ParentMessageId { get; set; }
    public string SenderId { get; set; }
    public int ChatId { get; set; }
    public string Content { get; set; }
    public DateTime DateSent { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public Message ParentMessage { get; set; }
    public User Sender { get; set; }
    public Chat Chat { get; set; }
}