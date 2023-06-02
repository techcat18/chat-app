using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ChatApplication.Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace ChatApplication.Functions;

public static class DatabaseManipulationFunctions
{
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
}