using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiWithAPI.Models;
using MauiWithAPI.Services;
using MauiWithAPI.ViewModels.Components;
using System.Collections.ObjectModel;

namespace MauiWithAPI.ViewModels
{
    public partial class CategoryPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<Category> categories = new();

        [ObservableProperty]
        public int page = 1;

        [ObservableProperty]
        public int pageSize = 5;

        [ObservableProperty]
        public int totalPages;

        [ObservableProperty]
        public string keyword = null;

        private readonly ICategoryService _categoryService;
        private readonly CreateToast toast = new();
        public CategoryPageViewModel(ICategoryService categoryService, IAuthenService authenService)
            : base(authenService)
        {
            _categoryService = categoryService;
        }
        [RelayCommand]
        public async Task GetAllCategory()
        {
            var result = await _categoryService.GetCategoriesAsync(Page, PageSize, Keyword);
            if (Categories.Count > 0)
            {
                Categories.Clear();
            }
            if (result.TotalItems > 0)
            {
                TotalPages = result.TotalPages;
                foreach (var category in result.Data)
                {
                    Categories.Add(category);
                }
            }
        }

        public async Task AddCategory(Category category)
        {
            if (category is null)
            {
                return;
            }
            var result = await _categoryService.AddCategoryAsync(category);
            if (result != null)
            {
                toast.CreateToastShow("Add successful");
                await GetAllCategory();
            }
            else
            {
                await AppConstants.NavigateToAuthenPage();
            }

        }

        public async Task UpdateCategory(Category category)
        {
            var result = await _categoryService.UpdateCategoryAsync(category);
            if (result != null)
            {
                toast.CreateToastShow("Update successful");
                await GetAllCategory();
            }
            else await AppConstants.NavigateToAuthenPage();
        }

        public async Task DeleteCategory(int Id)
        {
            var result = await _categoryService.DeleteCategoryAsync(Id);
            if (result == AppConstants.Unauthorized)
            {
                await AppConstants.NavigateToAuthenPage();
            }
            else
            {
                if (result is null)
                {
                    toast.CreateToastShow("Delete successful");
                    await GetAllCategory();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", $"{result}", "OK");
                }
            }
            //toast.CreateToastShow("Delete false");
        }
    }
}
