using MauiWithAPI.CustomControls;
using MauiWithAPI.ViewModels;

namespace MauiWithAPI.Pages;

public partial class UserPage : MasterContentPage
{
    private readonly UserPageViewModel _userPageViewModel;
    public UserPage(UserPageViewModel userPageViewModel)
    {
        InitializeComponent();
        BindingContext = userPageViewModel;
        _userPageViewModel = userPageViewModel;
    }
    protected override void OnAppearing()
    {
        _userPageViewModel.GetAllUsersCommand.Execute(null);

        pageSize.SelectedIndex = 0;
        previous.IsEnabled = true;
        next.IsEnabled = true;

        base.OnAppearing();
    }
    private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        _userPageViewModel.Keyword = ((SearchBar)sender).Text;
        _ = _userPageViewModel.GetAllUsers();
    }

    private void pageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectPageSize = ((Picker)sender).SelectedItem;
        _userPageViewModel.PageSize = (int)selectPageSize;
        _ = _userPageViewModel.GetAllUsers();
    }

    private void next_Clicked(object sender, EventArgs e)
    {
        if (_userPageViewModel.Page == _userPageViewModel.TotalPages)
        {
            next.IsEnabled = false;
        }
        else
        {
            next.IsEnabled = true;
            previous.IsEnabled = true;
            _userPageViewModel.Page++;
            _ = _userPageViewModel.GetAllUsers();
        }
    }

    private void previous_Clicked(object sender, EventArgs e)
    {
        if (_userPageViewModel.Page == 1)
            previous.IsEnabled = false;
        else
        {
            previous.IsEnabled = true;
            next.IsEnabled = true;
            _userPageViewModel.Page--;
            _ = _userPageViewModel.GetAllUsers();
        }
    }
}