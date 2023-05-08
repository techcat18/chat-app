namespace ChatApplication.Shared.Models.Message;

public class CreateMessageModel
{
    public int? ParentMessageId { get; set; }
    public string SenderId { get; set; }
    public int ChatId { get; set; }
    public string Content { get; set; }
}