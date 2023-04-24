using ChatApplication.Blazor.Helpers.Interfaces;
using ChatApplication.Blazor.Models.User;
using ChatApplication.Blazor.Services.Interfaces;
using ChatApplication.Shared.Models;
using Newtonsoft.Json;

namespace ChatApplication.Blazor.Services;

public class UserService: IUserService
{
    private readonly IApiHelper _apiHelper;

    public UserService(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<PagedList<UserModel>> GetByFilterAsync(UserFilterModel filterModel)
    {
        var userResponse = 
            await _apiHelper.GetAsync(filterModel, "users");

        var users = JsonConvert.DeserializeObject<PagedList<UserModel>>(userResponse);
        
        return users;
    }

    public async Task<UserModel> GetByIdAsync(string id)
    {
        var userModel =
            await _apiHelper.GetAsync<UserModel>($"users/{id}");

        return userModel;
    }
}