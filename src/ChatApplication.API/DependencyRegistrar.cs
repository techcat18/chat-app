using ChatApplication.BLL.Models.Auth;

namespace ChatApplication.API;

public static class DependencyRegistrar
{
    public static IServiceCollection ConfigureApiLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureOptions(configuration);
        
        return services;
    }
    
    private static void ConfigureOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
    }
}