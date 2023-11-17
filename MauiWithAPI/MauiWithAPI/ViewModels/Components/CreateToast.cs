using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace MauiWithAPI.ViewModels.Components;

public class CreateToast
{
    public async void CreateToastShow(string v)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string text = v;
        ToastDuration duration = ToastDuration.Short;
        double size = 14;

        var toast = Toast.Make(text, duration, size);
        await toast.Show(cancellationTokenSource.Token);
    }
}

