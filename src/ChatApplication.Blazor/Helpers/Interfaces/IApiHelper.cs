using ChatApplication.Blazor.Models.File;

namespace ChatApplication.Blazor.Helpers.Interfaces;

public interface IApiHelper
{
    Task<T> GetAsync<T>(string endpoint);
    Task<string> GetAsync<TIn>(TIn model, string endpoint);
    Task PostAsync<T>(T model, string endpoint);
    Task<TOut> PostAsync<TIn, TOut>(TIn model, string endpoint);
    Task FunctionPostAsync(FileUploadModel fileUploadModel, string endpoint);
    Task PutAsync<TIn>(TIn model, string endpoint);
    Task<TOut> PutAsync<TIn, TOut>(TIn model, string endpoint);
}