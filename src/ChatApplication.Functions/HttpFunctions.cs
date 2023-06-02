using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChatApplication.Functions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ChatApplication.Functions;

public static class HttpFunctions
{
    [FunctionName("wordDocuments")]
    public static async Task<IActionResult> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        var file = req.Form.Files.FirstOrDefault();

        if (file == null)
        {
            return new BadRequestObjectResult("No file");
        }
            
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var fileInputModel = new FileInputModel
        {
            FileBytes = memoryStream.ToArray(),
            FileName = file.FileName
        };
            
        var jsonString = JsonConvert.SerializeObject(fileInputModel);
                
        var instanceId = await starter.StartNewAsync(nameof(WordDocumentUploaderFunction.RunOrchestrator), input: jsonString);

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}