using System.Threading.Tasks;
using ChatApplication.Functions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
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
            
            var fileName = await context.CallActivityAsync<string>(
                nameof(BlobStorageManipulationFunctions.UploadFileToBlobStorage), fileInputModel);
            
            var databaseUploadModel = await context.CallActivityAsync<DatabaseUploadModel>(
                nameof(WordDocumentManipulationFunctions.ExtractTextFromFile), fileName);
            
            await context.CallActivityAsync(
                nameof(DatabaseManipulationFunctions.UploadTextToDatabase), databaseUploadModel);
            
            var retrievedText = await context.CallActivityAsync<string>(
                nameof(DatabaseManipulationFunctions.RetrieveTextFromDatabase), fileName);

            return new ObjectResult(retrievedText);
        }
    }
}