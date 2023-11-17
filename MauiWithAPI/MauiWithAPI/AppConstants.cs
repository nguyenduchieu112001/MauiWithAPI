using MauiWithAPI.Pages;

namespace MauiWithAPI
{
    public static class AppConstants
    {
        public const string HttpCategory = "https://localhost:7058/api/v1/Category";
        public const string HttpProduct = "https://localhost:7058/api/v1/Product";
        public const string HttpLogin = "https://localhost:7058/api/v1/Authen/login";
        public const string HttpUser = "https://localhost:7058/api/v1/User";
        public const string HttpRegister = "https://localhost:7058/api/v1/Authen/register";
        public const string HttpGetCurrentLoginInformations =
            "https://localhost:7058/api/v1/Authen/getCurrentLoginInformations";
        public const string HttpImage = "https://localhost:7058/Resources";
        public const string AuthStorageKeyName = "login-flow-auth-key";
        public const string TokenExpirationKeyName = "expiration-flow-auth-key";

        public const string Unauthorized = "Unauthorized";

        public static async Task NavigateToUserPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(UserPage)}");
        }

        public static async Task NavigateToAuthenPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(AuthenPage)}");
        }
    }
}
