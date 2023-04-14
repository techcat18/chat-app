using ChatApplication.API;
using ChatApplication.BLL;
using ChatApplication.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureDataAccessLayer(builder.Configuration);
builder.Services.ConfigureBusinessLogicLayer();
builder.Services.ConfigureApiLayer(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();

app.MapControllers();

app.Run();