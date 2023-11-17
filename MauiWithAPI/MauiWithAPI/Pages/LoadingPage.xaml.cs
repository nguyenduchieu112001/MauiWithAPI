using MauiWithAPI.ViewModels;

namespace MauiWithAPI.Pages;

public partial class LoadingPage : ContentPage
{
    private readonly LoadingPageViewModel _loadingPageViewModel;

    [Obsolete]
    public LoadingPage(LoadingPageViewModel loadingPageViewModel)
    {
        InitializeComponent();
        BindingContext = loadingPageViewModel;
        _loadingPageViewModel = loadingPageViewModel;

        IsLogin();
    }

    [Obsolete]
    private async void IsLogin()
    {
        await Device.InvokeOnMainThreadAsync(async () =>
        {
            await Task.Delay(3000); // Delay for 3 seconds

            var result = await _loadingPageViewModel.IsLogin();
            if (result)
            {
                await AppConstants.NavigateToUserPage();
            }
            else
            {
                await AppConstants.NavigateToAuthenPage();
            }
        });
    }
}