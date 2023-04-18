using ChatApplication.Blazor.Models.Chat;

namespace ChatApplication.Blazor.Services.Interfaces;

public interface IChatService
{
    Task<IEnumerable<ChatModel>> GetChatsAsync();
}