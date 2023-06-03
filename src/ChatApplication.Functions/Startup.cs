using System;
using Azure.Identity;
using ChatApplication.Functions;
using ChatApplication.Functions.Services;
using ChatApplication.Functions.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace ChatApplication.Functions;

public class Startup: IWebJobsStartup
{
    public void Configure(IWebJobsBuilder builder)
    {
        var keyVaultUrl = Environment.GetEnvironmentVariable("AzureKeyVaultUrl");

        var configuration = new ConfigurationBuilder()
            .AddAzureKeyVault(new Uri(keyVaultUrl!), new DefaultAzureCredential())
            .Build();

        builder.Services.AddSingleton<IConfiguration>(configuration);

        builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
    }
}