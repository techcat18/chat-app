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

    public static UserManager<User> CreateUserManager(ChatDbContext context)
    {
        var store = new UserStore<User>(context);
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<UserManager<User>>();
        
        return new UserManager<User>(
            store, 
            null, 
            new PasswordHasher<User>(),
            null, 
            null, 
            null, 
            null, 
            null, 
            logger);
    }

    public static SignInManager<User> CreateSignInManager(UserManager<User> userManager)
    {
        var httpContextAccessor = new HttpContextAccessor();
        var httpContext = new DefaultHttpContext();
        httpContextAccessor.HttpContext = httpContext;

        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<SignInManager<User>>();
        
        return new SignInManager<User>(
            userManager,
            httpContextAccessor,
            new UserClaimsPrincipalFactory<User>(userManager, new OptionsManager<IdentityOptions>(new OptionsFactory<IdentityOptions>(new List<IConfigureOptions<IdentityOptions>>(), new List<IPostConfigureOptions<IdentityOptions>>()))),
            null,
            logger,
            null,
            null);
    }

    public static IJwtService CreateJwtService(UserManager<User> userManager)
    {
        var jwtSettings = new JwtSettings();
        var options = Options.Create(jwtSettings);
        return new JwtService(options, userManager);
    }
}