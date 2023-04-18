using System.Text;
using ChatApplication.BLL.Models.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ChatApplication.API;

public static class DependencyRegistrar
{
    public static IServiceCollection ConfigureApiLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureOptions(configuration);
        services.ConfigureAuth(configuration);
        
        return services;
    }
    
    private static void ConfigureOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
    }

    private static void ConfigureAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthorization();
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration.GetSection("JwtSettings")["Issuer"],
                    ValidAudience = configuration.GetSection("JwtSettings")["Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings")["Key"]))
                };
            });
    }
}