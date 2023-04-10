using ChatApplication.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApplication.DAL.Configurations;

public class GroupChatConfiguration: IEntityTypeConfiguration<GroupChat>
{
    public void Configure(EntityTypeBuilder<GroupChat> builder)
    {
        builder.HasIndex(gc => gc.Name)
            .IsUnique();
        
        builder.Property(gc => gc.Name)
            .IsRequired()
            .HasMaxLength(1000);
        
        builder.HasQueryFilter(gc => !gc.IsDeleted);
    }
}