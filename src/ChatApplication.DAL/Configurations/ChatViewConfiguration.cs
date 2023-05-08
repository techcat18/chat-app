using ChatApplication.DAL.Entities.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApplication.DAL.Configurations;

public class ChatViewConfiguration: IEntityTypeConfiguration<ChatView>
{
    public void Configure(EntityTypeBuilder<ChatView> builder)
    {
        builder
            .ToView(nameof(ChatView));
    }
}