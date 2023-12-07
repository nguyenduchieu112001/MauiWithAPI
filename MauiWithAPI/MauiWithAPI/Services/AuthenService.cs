using MauiWithAPI.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace MauiWithAPI.Services
{
    public class AuthenService : IAuthenService
    {

        public AuthenService() { }
        public async Task<HttpClient> GetAuthenticatedHttpClientAsync(string uri)
        {
            var client = new HttpClient { BaseAddress = new Uri(uri) };
            var isTokenExpiration = await IsTokenExpiredAsync();
            if (!isTokenExpiration)
            {

                var token = await SecureStorage.Default.GetAsync(AppConstants.AuthStorageKeyName);
                if (!string.IsNullOrEmpty(token))
                {
                    token = token.Trim('"');
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                await Shell.Current.DisplayAlert("Alert!!!", "Session has expired", "OK");
                Logout();
            }
            return client;
        }

        public async Task<ApiResponse<T>> DeserializeApiResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return apiResponse;
        }

        private async Task<bool> IsTokenExpiredAsync()
        {
            var expirationTimeString = await SecureStorage.Default.GetAsync(AppConstants.TokenExpirationKeyName);
            if (!string.IsNullOrEmpty(expirationTimeString))
            {
                expirationTimeString = expirationTimeString.Trim('"');
                var expirationTime = DateTime.Parse(expirationTimeString);
                return expirationTime <= DateTime.Now;
            }
            return false;
        }

        public async Task<User> GetAuthenticatedUserAsync()
        {
            try
            {
                var client = await GetAuthenticatedHttpClientAsync(AppConstants.HttpGetCurrentLoginInformations);
                var response = await client.GetAsync(client.BaseAddress);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<User>(content,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    return apiResponse;
                }
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "Cannot connect to server", "OK");
            }
            return null;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var isTokenExpiration = await IsTokenExpiredAsync();
            return !isTokenExpiration;
        }

        public async Task<AuthResponse> LoginAsync(LoginForm dto)
        {
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri(AppConstants.HttpLogin) };
                var response = await client.PostAsJsonAsync(client.BaseAddress, dto);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<AuthResponse>(content,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    var token = JsonSerializer.Serialize(apiResponse.token);
                    var expiration = JsonSerializer.Serialize(apiResponse.expiration);
                    await SecureStorage.Default.SetAsync(AppConstants.AuthStorageKeyName, token);
                    await SecureStorage.Default.SetAsync(AppConstants.TokenExpirationKeyName, expiration);
                    return apiResponse;
                }
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "Cannot connect to server", "OK");
            }
            return null;
        }

        public async Task<string> RegisterAsync(RegisterForm registerForm)
        {
            try
            {
                var client = new HttpClient { BaseAddress = new Uri(AppConstants.HttpRegister) };
                var response = await client.PostAsJsonAsync(client.BaseAddress, registerForm);
                if (!response.IsSuccessStatusCode)
                {
                    var apiResponse = await DeserializeApiResponseAsync<string>(response);
                    return apiResponse.Errors.FirstOrDefault().ToString();
                }
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "Cannot connect to server", "OK");
            }
            return null;
        }

        public void Logout()
        {
            SecureStorage.Default.Remove(AppConstants.AuthStorageKeyName);
            SecureStorage.Default.Remove(AppConstants.TokenExpirationKeyName);
            _ = AppConstants.NavigateToAuthenPage();
        }
    }
}
