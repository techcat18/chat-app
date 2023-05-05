using Blazored.LocalStorage;
using ChatApplication.Blazor;
using ChatApplication.Blazor.Polly;
using Flurl.Http;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();

FlurlHttp.Configure(settings => settings.HttpClientFactory = new PollyHttpClientFactory());

builder.Services.ConfigureDependencies(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();