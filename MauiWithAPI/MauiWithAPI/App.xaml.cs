using MauiWithAPI.Services;
using MauiWithAPI.ViewModels;
using Microsoft.Maui.Controls.Handlers.Items;
using WindowsUI = Windows.UI;
using XamlUI = Microsoft.UI.Xaml.Media;
namespace MauiWithAPI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            AuthenService authenService = new();
            MainPage = new AppShell(new AuthPageViewModel(authenService));

            CollectionViewHandler.Mapper.AppendToMapping("HeaderAndFooterFix", (_, collectionView) =>
            {
                collectionView.AddLogicalChild(collectionView.Header as Element);
                collectionView.AddLogicalChild(collectionView.Footer as Element);
            });

#if WINDOWS10_0_19041_0_OR_GREATER
            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(IPicker.Title), (handler, view) =>
            {
                if (handler.PlatformView is not null && view is Picker pick && !String.IsNullOrWhiteSpace(pick.Title))
                {
                    handler.PlatformView.HeaderTemplate = new Microsoft.UI.Xaml.DataTemplate();
                    handler.PlatformView.PlaceholderText = pick.Title;
                    pick.Title = null;

                    pick.TitleColor.ToRgba(out byte r, out byte g, out byte b, out byte a);
                    handler.PlatformView.PlaceholderForeground = new XamlUI.SolidColorBrush(WindowsUI.Color.FromArgb(a, r, g, b));
                }
            });
#endif
        }
    }

}