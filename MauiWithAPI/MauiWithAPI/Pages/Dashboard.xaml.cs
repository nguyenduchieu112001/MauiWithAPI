using MauiWithAPI.CustomControls;
using MauiWithAPI.ViewModels;

namespace MauiWithAPI.Pages;

public partial class Dashboard : MasterContentPage
{
    public Dashboard(DashboardViewModel dashboardViewModel)
    {
        InitializeComponent();
        BindingContext = dashboardViewModel;

        //var timer = new System.Timers.Timer(1000);
        //timer.Elapsed += new ElapsedEventHandler(RedrawClock);
        //timer.Start();
    }

    //public void RedrawClock(object sender, ElapsedEventArgs e)
    //{
    //    var graphicsView = this.ClockGraphicsView;

    //    graphicsView.Invalidate();
    //}


}