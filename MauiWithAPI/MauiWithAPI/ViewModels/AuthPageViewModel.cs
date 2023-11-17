using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiWithAPI.Models;
using MauiWithAPI.Services;
using MauiWithAPI.ViewModels.Components;

namespace MauiWithAPI.ViewModels
{
    public partial class AuthPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private LoginForm loginForm = new();

        [ObservableProperty]
        private RegisterForm registerForm = new();

        private readonly CreateToast toast = new();
        private readonly IAuthenService _authenService;
        public AuthPageViewModel(IAuthenService authenService)
        {
            _authenService = authenService;
        }

        public AuthPageViewModel()
        {
        }

        public async Task<bool> Login()
        {
            var user = await _authenService.LoginAsync(LoginForm);
            if (user != null)
            {
                toast.CreateToastShow("Login success");
                return true;
            }
            else
            {
                toast.CreateToastShow("Invalid username or password");
                return false;
            }
        }

        public async Task<bool> Register()
        {
            var response = await _authenService.RegisterAsync(RegisterForm);
            if (response is null)
            {
                toast.CreateToastShow("Register success");
                return true;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", $"{response}", "OK");
                return false;
            }

        }
        public async Task<User> GetAuthenticatedUser()
        {
            var user = await _authenService.GetAuthenticatedUserAsync();
            return user;
        }

        [RelayCommand]
        public async Task Logout()
        {
            _authenService.Logout();
            await AppConstants.NavigateToAuthenPage();
        }

        public async Task<bool> IsLogin()
        {
            var isLogin = await _authenService.IsUserAuthenticated();
            return isLogin;
        }
    }
}
