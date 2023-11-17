using CommunityToolkit.Maui.Views;
using MauiWithAPI.Models;
using MauiWithAPI.ViewModels;

namespace MauiWithAPI.Pages.Components;

public partial class AddProductPopup : Popup
{
    public AddProductPopup(ProductPageViewModel productPageViewModel)
    {
        InitializeComponent();
        BindingContext = productPageViewModel;
    }

    private string _imageProduct;

    private bool CheckEntryProductName(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            App.Current.MainPage.DisplayAlert("Error", "Product Name cannot be empty", "Okey");
            return false;
        }
        return true;
    }
    private bool CheckEntryPrice(string price)
    {
        if (string.IsNullOrWhiteSpace(price))
        {
            App.Current.MainPage.DisplayAlert("Error", "Price cannot be empty", "Okey");
            return false;
        }
        if (!IsNumeric(price))
        {
            App.Current.MainPage.DisplayAlert("Error", "Price must be numeric", "Okey");
            return false;
        }
        return true;
    }
    private bool CheckSelectCategoryAsync(Category category)
    {
        if (category == null)
        {
            App.Current.MainPage.DisplayAlert("Error", "Category cannot be empty", "Okey");
            return false;
        }
        return true;
    }

    private bool CheckImageProduct()
    {
        if (imageProduct.Text == "Choose image")
        {
            App.Current.MainPage.DisplayAlert("Error", "Please choose image", "Okey");
            return false;
        }
        return true;
    }
    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        var category = categoryPicker.SelectedItem as Category;
        if (CheckEntryProductName(entryProductName.Text)
            && CheckEntryPrice(entryPrice.Text)
            && CheckSelectCategoryAsync(category)
            && CheckImageProduct())
        {
            var product = new Product
            {
                ProductName = entryProductName.Text,
                Price = int.Parse(entryPrice.Text.ToString()),
                CategoryId = category.Id,
                ProductImage = _imageProduct,
            };
            await CloseAsync(product);
        }
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private bool IsNumeric(string value)
    {
        return double.TryParse(value, out _);
    }

    private async void chooseImage_Clicked(object sender, EventArgs e)
    {
        var response = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Please select an image",
            FileTypes = FilePickerFileType.Images
        });
        if (response is null)
        {
            return;
        }
        if (response.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
            response.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) ||
            response.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase))
        {
            _imageProduct = response.FullPath;
            imageProduct.Text = response.FileName;
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Please choose file have end with (.jpg, .png, .jpeg)", "OK");
        }
    }
}