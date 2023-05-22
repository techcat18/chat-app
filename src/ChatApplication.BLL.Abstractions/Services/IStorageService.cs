using Microsoft.AspNetCore.Http;

namespace ChatApplication.BLL.Abstractions.Services;

public interface IStorageService
{
    Task<string> UploadAsync(Stream file, string containerName, string fileName);
    Task<string> GetSasTokenAsync(string containerName, string fileName);
    Task DeleteAsync(string blobFilename);
}