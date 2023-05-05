using ChatApplication.API;
using ChatApplication.API.Hubs;
using ChatApplication.BLL;
using ChatApplication.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        x => x
            .WithOrigins("https://localhost:7155")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.ConfigureDataAccessLayer(builder.Configuration);
builder.Services.ConfigureBusinessLogicLayer();
builder.Services.ConfigureApiLayer(builder.Configuration);

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseExceptionHandlingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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