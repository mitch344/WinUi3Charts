using Microsoft.UI.Xaml.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;

namespace WinUi3Charts.Controls
{
    public sealed partial class LineChart : UserControl
    {
        public Axis Axis { get; set; }
        private bool _isInitialLoad = true;
        private StackPanel _legendPanel;

        public Color AxisColor
        {
            get => (Color)GetValue(AxisColorProperty);
            set => SetValue(AxisColorProperty, value);
        }

        public Color LabelColor
        {
            get => (Color)GetValue(LabelColorProperty);
            set => SetValue(LabelColorProperty, value);
        }

        public FillStyle FillStyle
        {
            get => (FillStyle)GetValue(FillStyleProperty);
            set => SetValue(FillStyleProperty, value);
        }

        public string XAxisLabel
        {
            get => (string)GetValue(XAxisLabelProperty) ?? string.Empty;
            set => SetValue(XAxisLabelProperty, value);
        }

        public string YAxisLabel
        {
            get => (string)GetValue(YAxisLabelProperty) ?? string.Empty;
            set => SetValue(YAxisLabelProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty) ?? Enumerable.Empty<object>();
            set => SetValue(ItemsSourceProperty, value);
        }

        public string XValuePath
        {
            get => (string)GetValue(XValuePathProperty) ?? string.Empty;
            set => SetValue(XValuePathProperty, value);
        }

        public string YValuePath
        {
            get => (string)GetValue(YValuePathProperty) ?? string.Empty;
            set => SetValue(YValuePathProperty, value);
        }

        public Color LineColor
        {
            get => (Color)GetValue(LineColorProperty);
            set => SetValue(LineColorProperty, value);
        }

        public double LineThickness
        {
            get => (double)GetValue(LineThicknessProperty);
            set => SetValue(LineThicknessProperty, value);
        }

        public double? XAxisMin
        {
            get => (double?)GetValue(XAxisMinProperty);
            set => SetValue(XAxisMinProperty, value);
        }

        public double? XAxisMax
        {
            get => (double?)GetValue(XAxisMaxProperty);
            set => SetValue(XAxisMaxProperty, value);
        }

        public double? YAxisMin
        {
            get => (double?)GetValue(YAxisMinProperty);
            set => SetValue(YAxisMinProperty, value);
        }

        public double? YAxisMax
        {
            get => (double?)GetValue(YAxisMaxProperty);
            set => SetValue(YAxisMaxProperty, value);
        }

        public bool ShowDataPoints
        {
            get => (bool)GetValue(ShowDataPointsProperty);
            set => SetValue(ShowDataPointsProperty, value);
        }

        public double DataPointSize
        {
            get => (double)GetValue(DataPointSizeProperty);
            set => SetValue(DataPointSizeProperty, value);
        }

        public Color DataPointColor
        {
            get => (Color)GetValue(DataPointColorProperty);
            set => SetValue(DataPointColorProperty, value);
        }

        public Color? FillColor
        {
            get => (Color?)GetValue(FillColorProperty);
            set => SetValue(FillColorProperty, value);
        }

        public bool AnimateOnLoad
        {
            get => (bool)GetValue(AnimateOnLoadProperty);
            set => SetValue(AnimateOnLoadProperty, value);
        }

        public ObservableCollection<LineSeries> SeriesCollection
        {
            get => (ObservableCollection<LineSeries>)GetValue(SeriesCollectionProperty) ?? new ObservableCollection<LineSeries>();
            set => SetValue(SeriesCollectionProperty, value);
        }

        public bool ShowLegend
        {
            get => (bool)GetValue(ShowLegendProperty);
            set => SetValue(ShowLegendProperty, value);
        }

        public LegendPosition LegendPosition
        {
            get => (LegendPosition)GetValue(LegendPositionProperty);
            set => SetValue(LegendPositionProperty, value);
        }

        public string XAxisDateTimeFormat
        {
            get { return (string)GetValue(XAxisDateTimeFormatProperty); }
            set { SetValue(XAxisDateTimeFormatProperty, value); }
        }

        public string YAxisDateTimeFormat
        {
            get { return (string)GetValue(YAxisDateTimeFormatProperty); }
            set { SetValue(YAxisDateTimeFormatProperty, value); }
        }
    }
}
