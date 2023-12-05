using CommunityToolkit.Maui;
using MauiWithAPI.CustomControls;
using MauiWithAPI.Pages;
using MauiWithAPI.Services;
using MauiWithAPI.ViewModels;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MauiWithAPI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp(true)
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Services
            builder.Services.AddSingleton<ICategoryService, CategoryService>();
            builder.Services.AddSingleton<IAuthenService, AuthenService>();
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<IUserService, UserService>();

            // ViewModel
            builder.Services.AddTransient<BaseViewModel>();
            builder.Services.AddTransient<LoadingPageViewModel>();
            builder.Services.AddTransient<AuthPageViewModel>();
            builder.Services.AddTransient<UserPageViewModel>();
            builder.Services.AddTransient<ProductPageViewModel>();
            builder.Services.AddTransient<CategoryPageViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();

            //Pages
            builder.Services.AddTransient<MasterContentPage>();
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<AuthenPage>();
            builder.Services.AddTransient<UserPage>();
            builder.Services.AddTransient<ProductPage>();
            builder.Services.AddTransient<CategoryPage>();
            builder.Services.AddTransient<Dashboard>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}