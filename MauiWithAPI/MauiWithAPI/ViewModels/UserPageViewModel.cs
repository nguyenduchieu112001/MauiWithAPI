using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiWithAPI.Models;
using MauiWithAPI.Services;
using System.Collections.ObjectModel;

namespace MauiWithAPI.ViewModels
{
    public partial class UserPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<User> users = new();

        [ObservableProperty]
        public int page = 1;

        [ObservableProperty]
        public int pageSize = 5;

        [ObservableProperty]
        public int totalPages;

        [ObservableProperty]
        public string keyword;

        private readonly IUserService _userService;

        private readonly IMap _map;
        private readonly IConnectivity _connectivity;

        public UserPageViewModel(
            IUserService userService,
            IAuthenService authenService,
            IMap map,
            IConnectivity connectivity)
            : base(authenService)
        {
            _userService = userService;
            _map = map;
            _connectivity = connectivity;
        }

        [RelayCommand]
        public async Task GetAllUsers()
        {
            var result = await _userService.GetUsersAsync(Page, PageSize, Keyword);
            if (result is null)
            {
                await AppConstants.NavigateToAuthenPage();
                return;
            }
            if (Users.Count > 0)
            {
                Users.Clear();
            }
            if (result.TotalItems > 0)
            {
                TotalPages = result.TotalPages;
                foreach (var user in result.Data)
                {
                    Users.Add(user);
                }
            }
        }

        [RelayCommand]
        public async Task CheckLocation()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("ERROR", "You need internet connect for this", "OK");
                return;
            }

            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("ERROR", "You need internet connect for this", "OK");
                return;
            }

            var location = new Location(16.461834743798825, 107.59138400914395);
            var options = new MapLaunchOptions { Name = "Tòa nhà Viettel" };

            await _map.OpenAsync(location, options);
        }
    }
}
