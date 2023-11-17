using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiWithAPI.ViewModels
{
    public partial class LoadingPageViewModel : ObservableObject
    {
        public LoadingPageViewModel()
        {
        }

        private async Task<bool> IsTokenExpiredAsync()
        {
            var expirationTimeString = await SecureStorage.Default.GetAsync(AppConstants.TokenExpirationKeyName);
            if (!string.IsNullOrWhiteSpace(expirationTimeString))
            {
                expirationTimeString = expirationTimeString.Trim('"');
                var expirationTime = DateTime.Parse(expirationTimeString);
                return expirationTime <= DateTime.Now;
            }
            return true;
        }

        public async Task<bool> IsLogin()
        {
            var isLogin = await IsTokenExpiredAsync();
            return !isLogin;
        }
    }
}
