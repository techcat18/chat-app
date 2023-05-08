namespace ChatApplication.Shared.Models.Chat;

public class CreateChatModel
{
    public string Name { get; set; }
    public int ChatTypeId { get; set; }
    public IEnumerable<string> UserIds { get; set; }
}