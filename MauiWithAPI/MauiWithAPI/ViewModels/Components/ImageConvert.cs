using System.Globalization;

namespace MauiWithAPI.ViewModels.Components
{
    public class ImageConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imageUrl)
            {
                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    string imageProduct = $"{AppConstants.HttpImage}/{imageUrl}";
                    return ImageSource.FromUri(new Uri(imageProduct));
                }
            }

            return ImageSource.FromFile("dotnet_bot.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
