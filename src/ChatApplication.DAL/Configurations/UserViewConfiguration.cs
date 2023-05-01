using ChatApplication.DAL.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApplication.DAL.Configurations;

public class UserViewConfiguration: IEntityTypeConfiguration<UserView>
{
    public void Configure(EntityTypeBuilder<UserView> builder)
    {
        builder
            .ToView(nameof(UserView));
    }
}