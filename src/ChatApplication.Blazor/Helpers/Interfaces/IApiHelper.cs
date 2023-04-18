namespace ChatApplication.Blazor.Helpers.Interfaces;

public interface IApiHelper
{
    Task<T> GetAsync<T>(string endpoint);
    Task PostAsync<T>(T model, string endpoint);
    Task<TOut> PostAsync<TIn, TOut>(TIn model, string endpoint);
}