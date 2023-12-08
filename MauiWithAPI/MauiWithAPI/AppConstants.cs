using MauiWithAPI.Pages;

namespace MauiWithAPI
{
    public static class AppConstants
    {
        public const string BaseUri = "https://localhost:7058/api/v1";

        #region API URI
        public const string HttpCategory = $"{BaseUri}/Category";
        public const string HttpProduct = $"{BaseUri}/Product";
        public const string HttpLogin = $"{BaseUri}/Authen/login";
        public const string HttpUser = $"{BaseUri}/User";
        public const string HttpRegister = $"{BaseUri}/Authen/register";
        public const string HttpGetCurrentLoginInformations =
            $"{BaseUri}/Authen/getCurrentLoginInformations";
        public const string HttpImage = "https://localhost:7058/Resources";
        #endregion

        public const string AuthStorageKeyName = "login-flow-auth-key";
        public const string TokenExpirationKeyName = "expiration-flow-auth-key";

        public const string Unauthorized = "Unauthorized";

        #region Navigate To Page
        public static async Task NavigateToUserPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(UserPage)}");
        }

        public static async Task NavigateToAuthenPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(AuthenPage)}");
        }

        public static async Task NavigateToDashboard()
        {
            await Shell.Current.GoToAsync($"//{nameof(Dashboard)}");
        }
        #endregion
    }
}
