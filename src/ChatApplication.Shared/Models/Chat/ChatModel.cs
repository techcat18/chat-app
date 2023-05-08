namespace ChatApplication.Shared.Models.Chat;

public class ChatModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ChatTypeId { get; set; }
    public int MembersCount { get; set; }
}