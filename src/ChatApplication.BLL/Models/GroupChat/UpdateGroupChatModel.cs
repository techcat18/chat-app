using System.Text.Json.Serialization;

namespace ChatApplication.BLL.Models.GroupChat;

public class UpdateGroupChatModel
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; }
}