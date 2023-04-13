using System.Text.Json.Serialization;

namespace ChatApplication.BLL.Models.GroupChat;

public class UpdateChatModel
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
}