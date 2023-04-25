using ChatApplication.Blazor.Helpers;
using ChatApplication.Blazor.Helpers.Interfaces;
using ChatApplication.Blazor.Services;
using ChatApplication.Blazor.Services.Interfaces;

namespace ChatApplication.Blazor;

public static class DependencyRegistrar
{
    public static IServiceCollection ConfigureDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureServices();
        
        return services;
    }

    private static void ConfigureServices(
        this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ChatAuthenticationStateProvider>();
        services.AddScoped<IApiHelper, ApiHelper>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMessageService, MessageService>();
    }
}