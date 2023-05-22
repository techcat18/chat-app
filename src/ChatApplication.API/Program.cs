using Azure.Identity;
using ChatApplication.API;
using ChatApplication.API.Hubs;
using ChatApplication.BLL;
using ChatApplication.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

if (builder.Environment.IsProduction())
{
    var keyVaultUrl = new Uri(builder.Configuration.GetSection("AzureKeyVaultUrl").Value!);
    var azureCredential = new DefaultAzureCredential();
            
    builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCredential);
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        x => x
            .WithOrigins(builder.Configuration.GetSection("FrontUrl").Value!)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.ConfigureDataAccessLayer(builder.Configuration);
builder.Services.ConfigureBusinessLogicLayer(builder.Configuration);
builder.Services.ConfigureApiLayer(builder.Configuration);

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseExceptionHandlingMiddleware();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.Use(async (httpContext, next) =>
{
    var accessToken = httpContext.Request.Query["access_token"];

    var path = httpContext.Request.Path;
    
    if (!string.IsNullOrEmpty(accessToken) &&
        (path.StartsWithSegments("/api/videoHub")))
    {
        httpContext.Request.Headers["Authorization"] = "Bearer " + accessToken;
    }

    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/api/chatHub");
app.MapHub<VideoHub>("/api/videoHub");

app.MapControllers();

app.Run();