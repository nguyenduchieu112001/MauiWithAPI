using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiWithAPI.Models;
using MauiWithAPI.Services;
using MauiWithAPI.ViewModels.Components;
using System.Collections.ObjectModel;

namespace MauiWithAPI.ViewModels
{
    public partial class ProductPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<Product> products;

        [ObservableProperty]
        ObservableCollection<Category> categories;

        [ObservableProperty]
        public int page = 1;

        [ObservableProperty]
        public int pageSize = 5;

        [ObservableProperty]
        public int totalPages;

        [ObservableProperty]
        public string keyword;

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly CreateToast toast = new();
        public ProductPageViewModel(
            IProductService productService,
            ICategoryService categoryService,
            IAuthenService authenService) : base(authenService)
        {

            products = new();
            categories = new();

            _productService = productService;
            _categoryService = categoryService;
        }
        [RelayCommand]

        public async Task GetAllProducts()
        {
            var result = await _productService.GetProductsAsync(Page, PageSize, Keyword);
            if (Products.Count > 0)
            {
                Products.Clear();
            }
            if (result.TotalItems > 0)
            {
                TotalPages = result.TotalPages;
                foreach (var product in result.Data)
                {
                    Products.Add(product);
                }
            }
        }

        [RelayCommand]
        public async Task GetAllCategories()
        {
            var result = await _categoryService.GetCategoriesAsync(0, 0, null);
            if (Categories.Count > 0)
            {
                Categories.Clear();
            }
            if (result.TotalItems > 0)
            {
                foreach (var category in result.Data)
                {
                    Categories.Add(category);
                }
            }

        }

        public async Task AddProduct(Product product)
        {
            if (product is null)
            {
                return;
            }
            var result = await _productService.SaveProductAsync(product, true);
            if (result != null)
            {
                toast.CreateToastShow("Add successful");
                await GetAllProducts();
            }
            else
            {
                //toast.CreateToastShow("Add false");
                await AppConstants.NavigateToAuthenPage();
            }

        }

        public async Task UpdateProduct(Product product)
        {
            var result = await _productService.SaveProductAsync(product);
            if (result != null)
            {
                toast.CreateToastShow("Update successful");
                await GetAllProducts();
            }
            else await AppConstants.NavigateToAuthenPage();
            //toast.CreateToastShow("Update false");
        }

        public async Task DeleteProduct(int Id)
        {
            var result = await _productService.DeleteProductAsync(Id);
            if (result)
            {
                toast.CreateToastShow("Delete successful");
                await GetAllProducts();
            }
            else
            {
                await AppConstants.NavigateToAuthenPage();
            }
            //toast.CreateToastShow("Delete false");
        }
    }
}
