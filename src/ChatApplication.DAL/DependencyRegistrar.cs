using ChatApplication.DAL.Data;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories;
using ChatApplication.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApplication.DAL;

public static class DependencyRegistrar
{
    public static IServiceCollection ConfigureDataAccessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureRepositories();
        services.ConfigureUnitOfWork();
        services.ConfigureDbContext(configuration);
        services.ConfigureIdentity();

        return services;
    }

    private static void ConfigureRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUserChatRepository, UserChatRepository>();
    }

    private static void ConfigureUnitOfWork(
        this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void ConfigureDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ChatDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SQLConnection"));
        });
    }
    
    private static void ConfigureIdentity(
        this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = false;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ChatDbContext>();
    }
}