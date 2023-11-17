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

        public UserPageViewModel(
            IUserService userService,
            IAuthenService authenService)
            : base(authenService)
        {
            _userService = userService;
        }

        [RelayCommand]

        public async Task GetAllUsers()
        {
            var result = await _userService.GetUsersAsync(Page, PageSize, Keyword);
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
    }
}
