using ChatApplication.DAL.Data;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Repositories;
using ChatApplication.DAL.Repositories.Interfaces;
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

        return services;
    }

    private static void ConfigureRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IGroupChatRepository, GroupChatRepository>();
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
}