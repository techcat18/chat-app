using Azure.Storage.Blobs;
using ChatApplication.BLL.Abstractions.Services;
using ChatApplication.BLL.MappingProfiles;
using ChatApplication.BLL.Services;
using ChatApplication.BLL.Validators.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApplication.BLL;

public static class DependencyRegistrar
{
    public static IServiceCollection ConfigureBusinessLogicLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureServices();
        services.ConfigureAutomapper();
        services.ConfigureFluentValidation();
        services.ConfigureAzureBlobServiceClient(configuration);

        return services;
    }

    private static void ConfigureServices(
        this IServiceCollection services)
    {
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IStorageService, AzureBlobStorageService>();
        services.AddHttpContextAccessor();
    }

    private static void ConfigureAutomapper(
        this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ChatProfile));
    }

    private static void ConfigureFluentValidation(
        this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<LoginModelValidator>();
    }

    private static void ConfigureAzureBlobServiceClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped(_ => new BlobServiceClient(configuration.GetSection("Azure:Blob:ConnectionString").Value));
    }
}