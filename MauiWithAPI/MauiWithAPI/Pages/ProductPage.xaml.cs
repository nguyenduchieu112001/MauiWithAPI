using CommunityToolkit.Maui.Views;
using MauiWithAPI.CustomControls;
using MauiWithAPI.Models;
using MauiWithAPI.Pages.Components;
using MauiWithAPI.ViewModels;

namespace MauiWithAPI.Pages;

public partial class ProductPage : MasterContentPage
{
    private readonly ProductPageViewModel _productPageViewModel;
    public ProductPage(ProductPageViewModel productPageViewModel)
    {
        InitializeComponent();
        BindingContext = productPageViewModel;
        _productPageViewModel = productPageViewModel;
    }
    protected override void OnAppearing()
    {
        _productPageViewModel.GetAllProductsCommand.Execute(null);
        _productPageViewModel.GetAllCategoryCommand.Execute(null);

        pageSize.SelectedIndex = 0;
        previous.IsEnabled = true;
        next.IsEnabled = true;

        base.OnAppearing();
    }

    private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        _productPageViewModel.Keyword = ((SearchBar)sender).Text;
        _ = _productPageViewModel.GetAllProducts();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var popup = new AddProductPopup(_productPageViewModel);
        var product = await this.ShowPopupAsync(popup) as Product;
        await _productPageViewModel.AddProduct(product);
    }

    private void entryPrice_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            string newText = new(e.NewTextValue.Where(c => char.IsDigit(c)).ToArray());

            if (e.NewTextValue != newText)
            {
                ((Entry)sender).Text = newText;
            }
        }
    }

    private async void productName_Completed(object sender, EventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is Product product)
        {
            var updateProduct = new Product
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                CategoryId = product.Category.Id,
            };
            await _productPageViewModel.UpdateProduct(updateProduct);
        }
    }

    private void refreshButton_Clicked(object sender, EventArgs e)
    {
        productListView.ItemsSource = _productPageViewModel.Products;
    }

    private async void categoryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedCategory = (Category)picker.SelectedItem;
        var product = (Product)picker.BindingContext;
        var updateProduct = new Product
        {
            Id = product.Id,
            ProductName = product.ProductName,
            Price = product.Price,
            CategoryId = selectedCategory.Id,
        };

        await _productPageViewModel.UpdateProduct(updateProduct);
    }

    private async void deleteProduct_Clicked(object sender, EventArgs args)
    {
        var result = await DisplayAlert("Delete Product", "Do you want to delete?", "CANCEL", "DELETE");
        if (!result)
        {
            var product = ((ImageButton)sender).BindingContext as Product;
            await _productPageViewModel.DeleteProduct(product.Id);
        }
    }

    private async void imageProduct_Clicked(object sender, EventArgs e)
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
            var product = ((ImageButton)sender).BindingContext as Product;
            var updateProduct = new Product
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                ProductImage = response.FullPath,
                CategoryId = product.Category.Id,
            };
            await _productPageViewModel.UpdateProduct(updateProduct);
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Please choose file have end with (.jpg, .png, .jpeg)", "OK");
        }
    }

    private void pageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectPageSize = ((Picker)sender).SelectedItem;
        _productPageViewModel.PageSize = (int)selectPageSize;
        _ = _productPageViewModel.GetAllProducts();
    }

    private void next_Clicked(object sender, EventArgs e)
    {
        if (_productPageViewModel.Page == _productPageViewModel.TotalPages)
        {
            next.IsEnabled = false;
        }
        else
        {
            next.IsEnabled = true;
            previous.IsEnabled = true;
            _productPageViewModel.Page++;
            _ = _productPageViewModel.GetAllCategory();
        }
    }

    private void previous_Clicked(object sender, EventArgs e)
    {
        if (_productPageViewModel.Page == 1)
            previous.IsEnabled = false;
        else
        {
            previous.IsEnabled = true;
            next.IsEnabled = true;
            _productPageViewModel.Page--;
            _ = _productPageViewModel.GetAllCategory();
        }
    }
}