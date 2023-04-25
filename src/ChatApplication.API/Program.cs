using ChatApplication.API;
using ChatApplication.API.Hubs;
using ChatApplication.BLL;
using ChatApplication.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.ConfigureDataAccessLayer(builder.Configuration);
builder.Services.ConfigureBusinessLogicLayer();
builder.Services.ConfigureApiLayer(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/api/chathub");
app.MapControllers();

app.Run();