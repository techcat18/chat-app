using ChatApplication.Blazor.Models.File;

namespace ChatApplication.Blazor.Services.Interfaces;

public interface IWordDocumentService
{
    Task UploadFileAsync(FileUploadModel file);
}