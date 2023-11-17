using MauiWithAPI.Models;

namespace MauiWithAPI.Services
{
    public interface IAuthenService
    {
        Task<bool> IsUserAuthenticated();
        Task<AuthResponse> LoginAsync(LoginForm dto);
        Task<string> RegisterAsync(RegisterForm dto);
        Task<User> GetAuthenticatedUserAsync();
        Task<HttpClient> GetAuthenticatedHttpClientAsync(string uri);
        Task<ApiResponse<T>> DeserializeApiResponseAsync<T>(HttpResponseMessage response);
        void Logout();
    }
}
