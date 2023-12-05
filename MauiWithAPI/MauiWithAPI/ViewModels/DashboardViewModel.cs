using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using MauiWithAPI.Services;
using SkiaSharp;

namespace MauiWithAPI.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    public DashboardViewModel(IAuthenService authenService) : base(authenService)
    {

    }

    public ISeries[] LineSeries { get; set; } =
    {
        new LineSeries<int>
        {
            Values = new [] { 1, 6, 4, 2, 7, 4, 6, 8, 6, 7, 5, 7 },
            Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 4 },
            Fill = null,
            GeometryFill = new SolidColorPaint(SKColors.AliceBlue),
            GeometryStroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 4 }
        },
        new LineSeries<int>
        {
            Values = new [] { 7, 5, 3, 4, 1, 3, 3, 2, 3, 4, 2, 1 },
            Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 4 },
            Fill = null,
            GeometryFill = new SolidColorPaint(SKColors.IndianRed),
            GeometryStroke = new SolidColorPaint(SKColors.DarkSalmon) { StrokeThickness = 4 }
        }
    };

    public ISeries[] StackedColumnSeries { get; set; } =
    {
        new StackedColumnSeries<int>
        {
            Values = new List<int> { 1, 3, 4, 5, 6, 6, 7 },
            Stroke = null,
            DataLabelsPaint = new SolidColorPaint(SKColors.Blue),
            MaxBarWidth = 10,
            DataLabelsSize = 0,
            //DataLabelsPosition = DataLabelsPosition.Middle
        },
        new StackedColumnSeries<int>
        {
            Values = new List<int> { 5, 6, 5, 4, 3, 2, 1 },
            Stroke = null,
            DataLabelsPaint = new SolidColorPaint(SKColors.Red),
            MaxBarWidth = 10,
            DataLabelsSize = 0,
        },
        new StackedColumnSeries<int>
        {
            Values = new List<int> { 3, 2, 2, 1, 1, 1, 1 },
            Stroke = null,
            DataLabelsPaint = new SolidColorPaint(SKColors.SpringGreen),
            MaxBarWidth = 10,
            DataLabelsSize = 0,
        },
        new StackedColumnSeries<int>
        {
            Values = new List<int> {1, 2, 2, 1, 2, 3, 4 },
            Stroke = null,
            DataLabelsPaint = new SolidColorPaint(SKColors.AliceBlue),
            MaxBarWidth = 10,
            DataLabelsSize = 0,
        }
    };

    public IEnumerable<ISeries> GaugeSeries { get; set; } =
        GaugeGenerator.BuildSolidGauge(
            new GaugeItem(10, series => SetStyle("South area", series)),
            new GaugeItem(25, series => SetStyle("North area", series)),
            new GaugeItem(40, series => SetStyle("East area", series)),
            new GaugeItem(35, series => SetStyle("West area", series)),
            new GaugeItem(GaugeItem.Background, series =>
            {
                series.InnerRadius = 20;
                series.RelativeOuterRadius = 15;
            })
       );

    public static void SetStyle(string name, PieSeries<ObservableValue> series)
    {
        series.Name = name;
        series.DataLabelsPosition = PolarLabelsPosition.Start;
        series.DataLabelsFormatter =
                point => $"{point.Context.Series.Name} {point.Coordinate.PrimaryValue}";
        series.InnerRadius = 20;
        series.RelativeOuterRadius = 15;
    }
    [ObservableProperty]
    public static int processing = 81;
    [ObservableProperty]
    public static int failed = 106;
    [ObservableProperty]
    public static int completed = 461;

    public IEnumerable<ISeries> DoughnutSeries { get; set; } =
        new[] { processing, failed, completed }.AsPieSeries((value, series) =>
        {
            series.MaxRadialColumnWidth = 10;
        });

    public IEnumerable<ISeries> BasicGauge { get; set; } =
        GaugeGenerator.BuildSolidGauge(
            new GaugeItem(49, series =>
            {
                series.Fill = new SolidColorPaint(SKColors.CornflowerBlue);
                series.DataLabelsSize = 40;
                series.DataLabelsPaint = new SolidColorPaint(SKColors.Black);
                series.DataLabelsPosition = PolarLabelsPosition.ChartCenter;
                series.InnerRadius = 55;
            }),
            new GaugeItem(GaugeItem.Background, series =>
            {
                series.InnerRadius = 55;
                series.Fill = new SolidColorPaint(SKColors.WhiteSmoke);
            }));
}
