using Blazored.LocalStorage;
using ChatApplication.Blazor.Helpers.Interfaces;
using Flurl.Http;

namespace ChatApplication.Blazor.Helpers;

public class ApiHelper: IApiHelper
{
    private readonly ILocalStorageService _localStorage;
    private readonly string _apiUrl;

    public ApiHelper(
        ILocalStorageService localStorage,
        IConfiguration configuration)
    {
        _localStorage = localStorage;
        _apiUrl = configuration.GetSection("APIUrl").Value;
    }

    public async Task<T> GetAsync<T>(string endpoint)
    {
        var token = await _localStorage.GetItemAsync<string>("token");

        return await (_apiUrl + endpoint)
            .WithHeader("Authorization", $"Bearer {token}")
            .GetJsonAsync<T>();
    }

    public async Task<string> GetAsync<TIn>(TIn model, string endpoint)
    {
        var token = await _localStorage.GetItemAsync<string>("token");

        return await (_apiUrl + endpoint)
            .WithHeader("Authorization", $"Bearer {token}")
            .SetQueryParams(model)
            .GetStringAsync();
    }

    public async Task PostAsync<T>(T model, string endpoint)
    {
        var token = await _localStorage.GetItemAsync<string>("token");

        await (_apiUrl + endpoint)
            .WithHeader("Authorization", $"Bearer {token}")
            .PostJsonAsync(model);
    }

    public async Task<TOut> PostAsync<TIn, TOut>(TIn model, string endpoint)
    {
        var token = await _localStorage.GetItemAsync<string>("token");

        return await (_apiUrl + endpoint)
            .WithHeader("Authorization", $"Bearer {token}")
            .PostJsonAsync(model)
            .ReceiveJson<TOut>();
    }

    public async Task<TOut> PutAsync<TIn, TOut>(TIn model, string endpoint)
    {
        var token = await _localStorage.GetItemAsync<string>("token");

        return await (_apiUrl + endpoint)
            .WithHeader("Authorization", $"Bearer {token}")
            .PutJsonAsync(model)
            .ReceiveJson<TOut>();
    }
}