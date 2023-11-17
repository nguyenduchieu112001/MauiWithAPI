using MauiWithAPI.API.Services.Authentication.Dto;
using MauiWithAPI.API.Services.Users.Dto;

namespace MauiWithAPI.API.Services.Users
{
    public interface IUserService
    {
        Task<ApiResponse<PagedApiResponse<List<UserDto>>>> GetUsersAsync(int page, int pageSize, string keyword);
    }
}
