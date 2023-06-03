using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ChatApplication.Functions.Models;
using ChatApplication.Functions.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ChatApplication.Functions.Services;

public class BlobStorageService: IBlobStorageService
{
    private readonly IConfiguration _configuration;

    public BlobStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> UploadFileToBlobStorageAsync(FileInputModel model)
    {
        var storageConnectionString = _configuration.GetSection("AzureWebJobsStorage").Value;
        var blobServiceClient = new BlobServiceClient(storageConnectionString);
            
        var containerClient = blobServiceClient.GetBlobContainerClient("word-docs");

        if (!await containerClient.ExistsAsync())
        {
            await containerClient.CreateIfNotExistsAsync();
        }

        var fileName = $"{Guid.NewGuid()}_{model.FileName}";
            
        var blobClient = containerClient.GetBlobClient(fileName);

        var memoryStream = new MemoryStream();
        await memoryStream.WriteAsync(model.FileBytes.AsMemory(0, model.FileBytes.Length));
        memoryStream.Position = 0;

        await blobClient.UploadAsync(memoryStream);

        return fileName;
    }
}