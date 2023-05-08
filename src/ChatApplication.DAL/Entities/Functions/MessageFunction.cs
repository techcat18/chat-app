namespace ChatApplication.DAL.Entities.Functions;

public class MessageFunction
{
    public int Id { get; set; }
    public int? ParentMessageId { get; set; }
    public string SenderId { get; set; }
    public int ChatId { get; set; }
    public string Content { get; set; }
    public string? SenderProfilePicture { get; set; }
    public string SenderEmail { get; set; }
    public DateTime DateSent { get; set; }
}