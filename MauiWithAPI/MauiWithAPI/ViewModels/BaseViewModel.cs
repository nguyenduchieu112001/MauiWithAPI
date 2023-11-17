using CommunityToolkit.Mvvm.ComponentModel;
using MauiWithAPI.Services;

namespace MauiWithAPI.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _headerText;

        [ObservableProperty]
        private string _footerText;

        [ObservableProperty]
        private string _headerLogo;

        private readonly IAuthenService _authenService;
        public BaseViewModel(IAuthenService authenService)
        {
            _authenService = authenService;

            InitializeAsync();

            FooterText = "Copyright: .Net Maui 2023";
            HeaderLogo = "user.png";
        }

        private async void InitializeAsync()
        {
            var user = await _authenService.GetAuthenticatedUserAsync();
            HeaderText = user.Username;
        }
    }
}
