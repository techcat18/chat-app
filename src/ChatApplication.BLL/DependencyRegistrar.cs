using ChatApplication.BLL.MappingProfiles;
using ChatApplication.BLL.Services;
using ChatApplication.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApplication.BLL;

public static class DependencyRegistrar
{
    public static IServiceCollection ConfigureBusinessLogicLayer(
        this IServiceCollection services)
    {
        services.ConfigureServices();
        services.ConfigureAutomapper();

        return services;
    }

    private static void ConfigureServices(
        this IServiceCollection services)
    {
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
    }

    private static void ConfigureAutomapper(
        this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ChatProfile));
    }
}