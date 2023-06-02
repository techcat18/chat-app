using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ChatApplication.Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace ChatApplication.Functions;

public static class BlobStorageManipulationFunctions
{
    [FunctionName(nameof(UploadFileToBlobStorage))]
    public static async Task<string> UploadFileToBlobStorage(
        [ActivityTrigger] FileInputModel file,
        ILogger log)
    {
        var storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        var blobServiceClient = new BlobServiceClient(storageConnectionString);
            
        var containerClient = blobServiceClient.GetBlobContainerClient("word-docs");

        if (!await containerClient.ExistsAsync())
        {
            await containerClient.CreateIfNotExistsAsync();
        }

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            
        var blobClient = containerClient.GetBlobClient(fileName);

        var memoryStream = new MemoryStream();
        await memoryStream.WriteAsync(file.FileBytes.AsMemory(0, file.FileBytes.Length));
        memoryStream.Position = 0;

        await blobClient.UploadAsync(memoryStream);

        log.LogInformation($"File {file.FileName} uploaded to Blob Storage.");

        return fileName;
    }
}