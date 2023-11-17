using CommunityToolkit.Maui.Views;
using MauiWithAPI.CustomControls;
using MauiWithAPI.Models;
using MauiWithAPI.ViewModels;
using MyFirstMauiApp.Views.Components;

namespace MauiWithAPI.Pages;

public partial class CategoryPage : MasterContentPage
{
    private readonly CategoryPageViewModel _categoryPageViewModel;
    public CategoryPage(CategoryPageViewModel categoryPageViewModel)
    {
        InitializeComponent();
        BindingContext = categoryPageViewModel;
        _categoryPageViewModel = categoryPageViewModel;
    }

    protected override void OnAppearing()
    {
        _categoryPageViewModel.GetAllCategoryCommand.Execute(null);

        pageSize.SelectedIndex = 0;
        previous.IsEnabled = true;
        next.IsEnabled = true;

        base.OnAppearing();
    }

    private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        _categoryPageViewModel.Keyword = ((SearchBar)sender).Text;
        _ = _categoryPageViewModel.GetAllCategory();
    }

    private async void AddButton_Clicked(object sender, EventArgs e)
    {
        var popup = new AddCategoryPopup();
        var result = await this.ShowPopupAsync(popup) as Category;
        await _categoryPageViewModel.AddCategory(result);
    }

    private async void Entry_Completed(object sender, EventArgs e)
    {
        var updateCategory = new Category();
        if (sender is Entry entry && entry.BindingContext is Category category)
        {
            updateCategory.Id = category.Id;
            updateCategory.CategoryName = category.CategoryName;
            await _categoryPageViewModel.UpdateCategory(updateCategory);
        }
    }

    private void refreshButton_Clicked(object sender, EventArgs e)
    {
        categoryListView.ItemsSource = _categoryPageViewModel.Categories;
    }

    private async void pageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectPageSize = ((Picker)sender).SelectedItem;
        _categoryPageViewModel.PageSize = (int)selectPageSize;
        await _categoryPageViewModel.GetAllCategory();
    }

    private async void next_Clicked(object sender, EventArgs e)
    {
        if (_categoryPageViewModel.Page == _categoryPageViewModel.TotalPages)
        {
            next.IsEnabled = false;
        }
        else
        {
            next.IsEnabled = true;
            previous.IsEnabled = true;
            _categoryPageViewModel.Page++;
            await _categoryPageViewModel.GetAllCategory();
        }
    }

    private async void previous_Clicked(object sender, EventArgs e)
    {
        if (_categoryPageViewModel.Page == 1)
            previous.IsEnabled = false;
        else
        {
            previous.IsEnabled = true;
            next.IsEnabled = true;
            _categoryPageViewModel.Page--;
            await _categoryPageViewModel.GetAllCategory();
        }
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Delete Product", "Do you want to delete?", "DELETE", "CANCEL");
        if (result)
        {
            var category = ((ImageButton)sender).BindingContext as Category;
            await _categoryPageViewModel.DeleteCategory(category.Id);
        }
    }
}