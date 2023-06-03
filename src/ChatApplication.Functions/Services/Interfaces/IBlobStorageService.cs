using System.Threading.Tasks;
using ChatApplication.Functions.Models;

namespace ChatApplication.Functions.Services.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadFileToBlobStorageAsync(FileInputModel model);
}