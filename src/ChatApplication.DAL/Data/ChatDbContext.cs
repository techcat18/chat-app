using Microsoft.EntityFrameworkCore;

namespace ChatApplication.DAL.Contexts;

public class ChatDbContext: DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options): base(options){}
}