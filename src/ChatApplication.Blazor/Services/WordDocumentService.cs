using ChatApplication.Blazor.Helpers.Interfaces;
using ChatApplication.Blazor.Models.File;
using ChatApplication.Blazor.Services.Interfaces;

namespace ChatApplication.Blazor.Services;

public class WordDocumentService: IWordDocumentService
{
    private readonly IApiHelper _apiHelper;
    private readonly IConfiguration _configuration;

    public WordDocumentService(
        IApiHelper apiHelper,
        IConfiguration configuration)
    {
        _apiHelper = apiHelper;
        _configuration = configuration;
    }

    public async Task UploadFileAsync(FileUploadModel file)
    {
        await _apiHelper.FunctionPostAsync(file, _configuration.GetSection("AzureFunctionsUrl").Value);
    }
}