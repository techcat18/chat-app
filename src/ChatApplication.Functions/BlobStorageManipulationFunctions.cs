using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ChatApplication.Functions.Models;
using ChatApplication.Functions.Services.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace ChatApplication.Functions;

public class BlobStorageManipulationFunctions
{
    private readonly IBlobStorageService _blobStorageService;

    public BlobStorageManipulationFunctions(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }
    
    [FunctionName(nameof(UploadFileToBlobStorage))]
    public async Task<string> UploadFileToBlobStorage(
        [ActivityTrigger] FileInputModel file,
        ILogger log)
    {
        var fileName = await _blobStorageService.UploadFileToBlobStorageAsync(file);

        log.LogInformation($"File {file.FileName} uploaded to Blob Storage.");

        return fileName;
    }
}