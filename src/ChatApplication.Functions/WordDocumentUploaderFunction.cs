using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ChatApplication.Functions.Models;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ChatApplication.Functions
{
    public static class WordDocumentUploaderFunction
    {
        [FunctionName(nameof(RunOrchestrator))]
        public static async Task<IActionResult> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var jsonString = context.GetInput<string>();
            var fileInputModel = JsonConvert.DeserializeObject<FileInputModel>(jsonString);
            
            var fileName = await context.CallActivityAsync<string>(nameof(UploadFileToBlobStorage), fileInputModel);
            var databaseUploadModel = await context.CallActivityAsync<DatabaseUploadModel>(nameof(ExtractTextFromFile), fileName);
            await context.CallActivityAsync(nameof(UploadTextToDatabase), databaseUploadModel);
            var retrievedText = await context.CallActivityAsync<string>(nameof(RetrieveTextFromDatabase), fileName);

            return new ObjectResult(retrievedText);
        }

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

        [FunctionName(nameof(UploadTextToDatabase))]
        public static async Task UploadTextToDatabase(
            [ActivityTrigger] DatabaseUploadModel databaseUploadModel,
            ILogger log)
        {
            var databaseConnectionString = Environment.GetEnvironmentVariable("DBConnection");

            await using var sqlConnection = new SqlConnection(databaseConnectionString);
            
            sqlConnection.Open();

            var query = "INSERT INTO [Texts] ([Name], [Value]) VALUES (@Name, @Value)";

            await using var command = new SqlCommand(query, sqlConnection);

            command.Parameters.AddWithValue("Name", databaseUploadModel.Name);
            command.Parameters.AddWithValue("Value", databaseUploadModel.Value);

            command.ExecuteNonQuery();
                
            sqlConnection.Close();
            
            log.LogInformation("Text from the file uploaded to SQL Database.");
        }

        [FunctionName(nameof(RetrieveTextFromDatabase))]
        public static async Task<string> RetrieveTextFromDatabase(
            [ActivityTrigger] string name,
            ILogger log)
        {
            var databaseConnectionString = Environment.GetEnvironmentVariable("DBConnection");

            await using var sqlConnection = new SqlConnection(databaseConnectionString);
            
            sqlConnection.Open();

            var query = "SELECT [Value] FROM [Texts] WHERE [Name] = @Name";

            await using var command = new SqlCommand(query, sqlConnection);

            command.Parameters.AddWithValue("Name", name);

            var reader = await command.ExecuteReaderAsync();
            var text = string.Empty;

            while (reader.Read())
            {
                text = reader.GetString(0);
            }
            
            log.LogInformation("Text from SQL Database retrieved.");

            return text;
        }

        [FunctionName("WordDocumentUploaderFunction_HttpStart")]
        public static async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var file = req.Form.Files.FirstOrDefault();

            if (file == null)
            {
                return new BadRequestObjectResult("error");
            }
            
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileInputModel = new FileInputModel
            {
                FileBytes = memoryStream.ToArray(),
                FileName = file.FileName
            };
            
            var jsonString = JsonConvert.SerializeObject(fileInputModel);
                
            var instanceId = await starter.StartNewAsync(nameof(RunOrchestrator), input: jsonString);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}