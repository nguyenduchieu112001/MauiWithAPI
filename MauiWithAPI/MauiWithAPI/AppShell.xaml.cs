using MauiWithAPI.Pages;
using MauiWithAPI.ViewModels;

namespace MauiWithAPI
{
    public partial class AppShell : Shell
    {
        public AppShell(AuthPageViewModel authPageViewModel)
        {
            InitializeComponent();
            BindingContext = authPageViewModel;
            InitRoutes();
        }

        private void InitRoutes()
        {
            Routing.RegisterRoute(nameof(CategoryPage), typeof(CategoryPage));
            Routing.RegisterRoute(nameof(AuthenPage), typeof(AuthenPage));
            Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
            Routing.RegisterRoute(nameof(UserPage), typeof(UserPage));
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
        }
    }
}