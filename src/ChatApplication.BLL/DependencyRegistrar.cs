using Microsoft.Extensions.DependencyInjection;

namespace ChatApplication.BLL;

public static class DependencyRegistrar
{
    public static IServiceCollection ConfigureBusinessLogicLayer(
        this IServiceCollection services)
    {
        services.ConfigureServices();
        
        return services;
    }

    private static void ConfigureServices(
        this IServiceCollection services)
    {
        
    }
}