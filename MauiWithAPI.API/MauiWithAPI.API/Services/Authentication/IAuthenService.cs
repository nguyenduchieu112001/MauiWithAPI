using MauiWithAPI.API.Services.Authentication.Dto;

namespace MauiWithAPI.API.Services.Authentication
{
    public interface IAuthenService
    {
        Task<AuthResponse?> Login(LoginRequestDto loginRequestDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<string>> Register(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken = default);
        Task<UserDto> GetCurrentLoginInformations(HttpContext httpContext);
    }
}
