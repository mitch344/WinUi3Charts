using Microsoft.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using System.Collections;
using Windows.UI;
using System.Collections.ObjectModel;

namespace WinUi3Charts.Controls
{
    public sealed partial class LineChart : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(LineChart), new PropertyMetadata(null, OnItemsSourceChanged));

        public static readonly DependencyProperty XValuePathProperty =
            DependencyProperty.Register(nameof(XValuePath), typeof(string), typeof(LineChart), new PropertyMetadata(string.Empty, OnPathChanged));

        public static readonly DependencyProperty YValuePathProperty =
            DependencyProperty.Register(nameof(YValuePath), typeof(string), typeof(LineChart), new PropertyMetadata(string.Empty, OnPathChanged));

        public static readonly DependencyProperty XAxisMinProperty =
            DependencyProperty.Register(nameof(XAxisMin), typeof(double?), typeof(LineChart), new PropertyMetadata(null, OnAppearanceChanged));

        public static readonly DependencyProperty XAxisMaxProperty =
            DependencyProperty.Register(nameof(XAxisMax), typeof(double?), typeof(LineChart), new PropertyMetadata(null, OnAppearanceChanged));

        public static readonly DependencyProperty YAxisMinProperty =
            DependencyProperty.Register(nameof(YAxisMin), typeof(double?), typeof(LineChart), new PropertyMetadata(null, OnAppearanceChanged));

        public static readonly DependencyProperty YAxisMaxProperty =
            DependencyProperty.Register(nameof(YAxisMax), typeof(double?), typeof(LineChart), new PropertyMetadata(null, OnAppearanceChanged));

        public static readonly DependencyProperty LineThicknessProperty =
            DependencyProperty.Register(nameof(LineThickness), typeof(double), typeof(LineChart), new PropertyMetadata(2.0, OnAppearanceChanged));

        public static readonly DependencyProperty FillStyleProperty =
            DependencyProperty.Register(nameof(FillStyle), typeof(FillStyle), typeof(LineChart), new PropertyMetadata(FillStyle.Flat, OnAppearanceChanged));

        public static readonly DependencyProperty ShowDataPointsProperty =
            DependencyProperty.Register(nameof(ShowDataPoints), typeof(bool), typeof(LineChart), new PropertyMetadata(false, OnAppearanceChanged));

        public static readonly DependencyProperty DataPointSizeProperty =
            DependencyProperty.Register(nameof(DataPointSize), typeof(double), typeof(LineChart), new PropertyMetadata(6.0, OnAppearanceChanged));

        public static readonly DependencyProperty XAxisLabelProperty =
            DependencyProperty.Register(nameof(XAxisLabel), typeof(string), typeof(LineChart), new PropertyMetadata(string.Empty, OnAppearanceChanged));

        public static readonly DependencyProperty YAxisLabelProperty =
            DependencyProperty.Register(nameof(YAxisLabel), typeof(string), typeof(LineChart), new PropertyMetadata(string.Empty, OnAppearanceChanged));

        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register(nameof(LineColor), typeof(Color), typeof(LineChart), new PropertyMetadata(Colors.Blue, OnAppearanceChanged));

        public static readonly DependencyProperty FillColorProperty =
            DependencyProperty.Register(nameof(FillColor), typeof(Color?), typeof(LineChart), new PropertyMetadata(null, OnAppearanceChanged));

        public static readonly DependencyProperty DataPointColorProperty =
             DependencyProperty.Register(nameof(DataPointColor), typeof(Color), typeof(LineChart), new PropertyMetadata(Colors.Red, OnAppearanceChanged));

        public static readonly DependencyProperty AxisColorProperty =
            DependencyProperty.Register(nameof(AxisColor), typeof(Color), typeof(LineChart), new PropertyMetadata(Colors.White, OnAppearanceChanged));

        public static readonly DependencyProperty LabelColorProperty =
            DependencyProperty.Register(nameof(LabelColor), typeof(Color), typeof(LineChart), new PropertyMetadata(Colors.White, OnAppearanceChanged));

        public static readonly DependencyProperty AnimateOnLoadProperty =
            DependencyProperty.Register(nameof(AnimateOnLoad), typeof(bool), typeof(LineChart), new PropertyMetadata(false, OnAppearanceChanged));

        public static readonly DependencyProperty SeriesCollectionProperty =
            DependencyProperty.Register(nameof(SeriesCollection), typeof(ObservableCollection<LineSeries>), typeof(LineChart), new PropertyMetadata(null, OnSeriesCollectionChanged));

        public static readonly DependencyProperty ShowLegendProperty =
            DependencyProperty.Register(nameof(ShowLegend), typeof(bool), typeof(LineChart), new PropertyMetadata(true, OnAppearanceChanged));

        public static readonly DependencyProperty LegendPositionProperty =
            DependencyProperty.Register(nameof(LegendPosition), typeof(LegendPosition), typeof(LineChart), new PropertyMetadata(LegendPosition.Right, OnAppearanceChanged));

        public static readonly DependencyProperty XAxisDateTimeFormatProperty =
            DependencyProperty.Register(nameof(XAxisDateTimeFormatProperty), typeof(string), typeof(LineChart), new PropertyMetadata("d MMM"));

        public static readonly DependencyProperty YAxisDateTimeFormatProperty =
            DependencyProperty.Register(nameof(YAxisDateTimeFormatProperty), typeof(string), typeof(LineChart), new PropertyMetadata("d MMM"));
    }
}
