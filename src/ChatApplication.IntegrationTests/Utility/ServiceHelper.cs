using AutoMapper;
using ChatApplication.BLL.Abstractions.Services;
using ChatApplication.BLL.MappingProfiles;
using ChatApplication.BLL.Services;
using ChatApplication.DAL.Data;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Entities;
using ChatApplication.DAL.Repositories;
using ChatApplication.DAL.Repositories.Interfaces;
using ChatApplication.Shared.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ChatApplication.IntegrationTests.Utility;

public static class ServiceHelper
{
    public static ServiceProvider RegisterServices(ChatDbContext context)
    {
        var services = new ServiceCollection();
        
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped(_ => context);
        services.AddHttpContextAccessor();
        services.AddLogging(config =>
        {
            config.AddConsole();
        });
        
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
        
        var serviceProvider = services.BuildServiceProvider();

        return serviceProvider;
    }

    public static IUnitOfWork CreateUnitOfWork(
        ChatDbContext context, 
        ServiceProvider serviceProvider)
    {
        return new UnitOfWork(context, serviceProvider);
    }

    public static IMapper CreateMapper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfiles(new List<Profile>
            {
                new ChatProfile(),
                new UserProfile(),
                new MessageProfile()
            })));
    }

    public static UserManager<User> CreateUserManager(ServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        return userManager;
    }

    public static SignInManager<User> CreateSignInManager(
        ServiceProvider serviceProvider)
    {
        var signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();
        return signInManager;
    }

    public static IJwtService CreateJwtService(UserManager<User> userManager)
    {
        var jwtSettings = new JwtSettings();
        var options = Options.Create(jwtSettings);
        return new JwtService(options, userManager);
    }
}