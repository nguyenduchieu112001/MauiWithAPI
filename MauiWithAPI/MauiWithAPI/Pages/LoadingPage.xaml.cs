using MauiWithAPI.ViewModels;

namespace MauiWithAPI.Pages;

public partial class LoadingPage : ContentPage
{
    private readonly LoadingPageViewModel _loadingPageViewModel;

    public LoadingPage(LoadingPageViewModel loadingPageViewModel)
    {
        InitializeComponent();
        BindingContext = loadingPageViewModel;
        _loadingPageViewModel = loadingPageViewModel;

        IsLogin();
    }

    private async void IsLogin()
    {
        await Task.Delay(2000);

        var result = await _loadingPageViewModel.IsLogin();
        if (result)
        {
            await AppConstants.NavigateToDashboard();
        }
        else
        {
            await AppConstants.NavigateToAuthenPage();
        }
    }
}