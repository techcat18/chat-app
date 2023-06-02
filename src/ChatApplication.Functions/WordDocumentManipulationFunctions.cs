using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ChatApplication.Functions.Models;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace ChatApplication.Functions;

public static class WordDocumentManipulationFunctions
{
    [FunctionName(nameof(ExtractTextFromFile))]
    public static async Task<DatabaseUploadModel> ExtractTextFromFile(
        [ActivityTrigger] string fileName,
        ILogger log)
    {
        var storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        var blobServiceClient = new BlobServiceClient(storageConnectionString);
            
        var containerClient = blobServiceClient.GetBlobContainerClient("word-docs");

        var blobClient = containerClient.GetBlobClient(fileName);
        var stream = new MemoryStream();

        await blobClient.DownloadToAsync(stream);

        var fileBytes = stream.ToArray();

        using var memoryStream = new MemoryStream(fileBytes);
        using var wordDocument = WordprocessingDocument.Open(memoryStream, false);
        var mainPart = wordDocument.MainDocumentPart;
                    
        var documentText = string.Empty;

        if (mainPart?.Document.Body != null)
        {
            documentText = mainPart.Document.Body.InnerText;
        }
            
        log.LogInformation($"Text from the file {fileName} extracted.");

        return new DatabaseUploadModel
        {
            Name = fileName,
            Value = documentText
        };
    }
}