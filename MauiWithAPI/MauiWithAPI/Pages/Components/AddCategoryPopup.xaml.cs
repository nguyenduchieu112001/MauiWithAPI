using CommunityToolkit.Maui.Views;
using MauiWithAPI;
using MauiWithAPI.Models;

namespace MyFirstMauiApp.Views.Components;

public partial class AddCategoryPopup : Popup
{
    public AddCategoryPopup()
    {
        InitializeComponent();
    }

    private bool CheckCategoryNameEntry(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            App.Current.MainPage.DisplayAlert("Error", "Category Name cannot be empty", "Okey");
            return false;
        }
        return true;
    }
    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (CheckCategoryNameEntry(entryCategoryName.Text))
        {
            var category = new Category()
            {
                CategoryName = entryCategoryName.Text
            };
            await CloseAsync(category);
        }
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}