using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections;
using System.Linq;
using Windows.Foundation;
using Windows.UI;

namespace WinUi3Charts.Controls
{
    public partial class PolarChart : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(PolarChart), new PropertyMetadata(null, OnPropertyChanged));

        public static readonly DependencyProperty ValuePathProperty =
            DependencyProperty.Register(nameof(ValuePath), typeof(string), typeof(PolarChart), new PropertyMetadata(string.Empty, OnPropertyChanged));

        public static readonly DependencyProperty LabelPathProperty =
            DependencyProperty.Register(nameof(LabelPath), typeof(string), typeof(PolarChart), new PropertyMetadata(string.Empty, OnPropertyChanged));

        public static readonly DependencyProperty RingCountProperty =
            DependencyProperty.Register(nameof(RingCount), typeof(int), typeof(PolarChart), new PropertyMetadata(5, OnPropertyChanged));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(double?), typeof(PolarChart), new PropertyMetadata(null, OnPropertyChanged));

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(PolarChart), new PropertyMetadata(0.0, OnPropertyChanged));

        public static readonly DependencyProperty ShowLegendProperty =
            DependencyProperty.Register(nameof(ShowLegend), typeof(bool), typeof(PolarChart), new PropertyMetadata(true, OnPropertyChanged));

        public static readonly DependencyProperty LegendPositionProperty =
            DependencyProperty.Register(nameof(LegendPosition), typeof(LegendPosition), typeof(PolarChart), new PropertyMetadata(LegendPosition.Right, OnPropertyChanged));

        public static readonly DependencyProperty ChartColorProperty =
            DependencyProperty.Register(nameof(ChartColor), typeof(Color), typeof(PolarChart), new PropertyMetadata(Colors.Blue, OnPropertyChanged));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty) ?? Enumerable.Empty<object>();
            set => SetValue(ItemsSourceProperty, value);
        }

        public string ValuePath
        {
            get => (string)GetValue(ValuePathProperty) ?? string.Empty;
            set => SetValue(ValuePathProperty, value);
        }

        public string LabelPath
        {
            get => (string)GetValue(LabelPathProperty) ?? string.Empty;
            set => SetValue(LabelPathProperty, value);
        }

        public int RingCount
        {
            get => (int)GetValue(RingCountProperty);
            set => SetValue(RingCountProperty, value);
        }

        public double? MaxValue
        {
            get => (double?)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
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

        public Color ChartColor
        {
            get => (Color)GetValue(ChartColorProperty);
            set => SetValue(ChartColorProperty, value);
        }

        private StackPanel _legendPanel;

        public PolarChart()
        {
            InitializeComponent();
            SizeChanged += PolarChart_SizeChanged;

            MainGrid = new Grid();
            ChartCanvas = new Canvas();
            MainGrid.Children.Add(ChartCanvas);

            _legendPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10)
            };

            MainGrid.Children.Add(_legendPanel);
            Grid.SetColumn(_legendPanel, 1);
            Grid.SetRow(_legendPanel, 1);

            Content = MainGrid;
        }

        private void PolarChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawChart();
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as PolarChart;
            chart?.DrawChart();
        }

        private void DrawChart()
        {
            ChartCanvas.Children.Clear();
            _legendPanel.Children.Clear();

            if (!ChartUtilities.ValidateData(ItemsSource, ValuePath, LabelPath))
            {
                ChartUtilities.ShowErrorMessage("Invalid or insufficient data to display the chart.", ChartCanvas);
                return;
            }

            if (ShowLegend)
            {
                DrawLegend();
                PositionLegend();
            }

            Rect availableSpace = CalculateAvailableSpace();
            if (availableSpace.Width <= 0 || availableSpace.Height <= 0)
            {
                ChartUtilities.ShowErrorMessage("Insufficient space to draw the chart.", ChartCanvas);
                return;
            }

            ResizeChartCanvas(availableSpace);

            double centerX = availableSpace.Width / 2;
            double centerY = availableSpace.Height / 2;
            double radius = Math.Min(centerX, centerY) * 0.9;

            DrawNumericalAxis(centerX, centerY, radius);
            DrawCategoryAxesAndLabels(centerX, centerY, radius);
            DrawData(centerX, centerY, radius);
        }

        private Rect CalculateAvailableSpace()
        {
            double leftMargin = 0, topMargin = 0, rightMargin = 0, bottomMargin = 0;

            if (ShowLegend && _legendPanel != null)
            {
                _legendPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                switch (LegendPosition)
                {
                    case LegendPosition.Left:
                        leftMargin = _legendPanel.DesiredSize.Width + 10;
                        break;
                    case LegendPosition.Top:
                        topMargin = _legendPanel.DesiredSize.Height + 10;
                        break;
                    case LegendPosition.Right:
                        rightMargin = _legendPanel.DesiredSize.Width + 10;
                        break;
                    case LegendPosition.Bottom:
                        bottomMargin = _legendPanel.DesiredSize.Height + 10;
                        break;
                }
            }

            double availableWidth = Math.Max(0, ActualWidth - leftMargin - rightMargin);
            double availableHeight = Math.Max(0, ActualHeight - topMargin - bottomMargin);

            return new Rect(
                leftMargin,
                topMargin,
                availableWidth,
                availableHeight
            );
        }

        private void ResizeChartCanvas(Rect availableSpace)
        {
            ChartCanvas.Width = availableSpace.Width;
            ChartCanvas.Height = availableSpace.Height;
            Canvas.SetLeft(ChartCanvas, availableSpace.Left);
            Canvas.SetTop(ChartCanvas, availableSpace.Top);
        }

        private void DrawNumericalAxis(double centerX, double centerY, double radius)
        {
            var values = ItemsSource.Cast<object>().Select(GetItemValue);
            double maxValue = MaxValue ?? values.Max();

            for (int i = 1; i <= RingCount; i++)
            {
                double ringRadius = radius * i / RingCount;
                var ellipse = new Ellipse
                {
                    Width = ringRadius * 2,
                    Height = ringRadius * 2,
                    Stroke = new SolidColorBrush(Colors.LightGray),
                    StrokeThickness = 1
                };

                Canvas.SetLeft(ellipse, centerX - ringRadius);
                Canvas.SetTop(ellipse, centerY - ringRadius);
                ChartCanvas.Children.Add(ellipse);

                double value = maxValue * i / RingCount;
                var textBlock = new TextBlock
                {
                    Text = value.ToString("F1"),
                    FontSize = 10,
                    Foreground = new SolidColorBrush(Colors.White)
                };

                textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                if (textBlock.DesiredSize.Width < ringRadius * 2 && textBlock.DesiredSize.Height < ringRadius * 2)
                {
                    double labelAngle = DegreesToRadians(StartAngle);
                    double labelX = centerX + (ringRadius - textBlock.DesiredSize.Width / 2) * Math.Cos(labelAngle);
                    double labelY = centerY - (ringRadius - textBlock.DesiredSize.Height / 2) * Math.Sin(labelAngle);

                    Canvas.SetLeft(textBlock, labelX - textBlock.DesiredSize.Width / 2);
                    Canvas.SetTop(textBlock, labelY - textBlock.DesiredSize.Height / 2);
                    ChartCanvas.Children.Add(textBlock);
                }
            }
        }

        private void DrawCategoryAxesAndLabels(double centerX, double centerY, double radius)
        {
            int count = ItemsSource.Cast<object>().Count();
            for (int i = 0; i < count; i++)
            {
                double angle = 2 * Math.PI * i / count + DegreesToRadians(StartAngle);
                double x = centerX + radius * Math.Cos(angle);
                double y = centerY - radius * Math.Sin(angle);

                var line = new Line
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = x,
                    Y2 = y,
                    Stroke = new SolidColorBrush(Colors.Gray),
                    StrokeThickness = 1
                };

                ChartCanvas.Children.Add(line);

                var item = ItemsSource.Cast<object>().ElementAt(i);
                string label = item.GetType().GetProperty(LabelPath).GetValue(item).ToString();

                var textBlock = new TextBlock
                {
                    Text = label,
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Colors.White)
                };

                double labelX = centerX + (radius + 20) * Math.Cos(angle) - textBlock.ActualWidth / 2;
                double labelY = centerY - (radius + 20) * Math.Sin(angle) - textBlock.ActualHeight / 2;

                Canvas.SetLeft(textBlock, labelX);
                Canvas.SetTop(textBlock, labelY);

                ChartCanvas.Children.Add(textBlock);
            }
        }

        private void DrawData(double centerX, double centerY, double radius)
        {
            var pointCollection = new PointCollection();

            var values = ItemsSource.Cast<object>().Select(GetItemValue);
            double maxValue = MaxValue ?? values.Max();
            int index = 0;
            foreach (var item in ItemsSource)
            {
                double angle = 2 * Math.PI * index / ItemsSource.Cast<object>().Count() + DegreesToRadians(StartAngle);
                double value = GetItemValue(item);
                double normalizedValue = value / maxValue;
                double x = centerX + radius * normalizedValue * Math.Cos(angle);
                double y = centerY - radius * normalizedValue * Math.Sin(angle);

                pointCollection.Add(new Point(x, y));
                index++;
            }

            var polygon = new Polygon
            {
                Points = pointCollection,
                Fill = new SolidColorBrush(ChartColor) { Opacity = 0.5 },
                Stroke = new SolidColorBrush(ChartColor),
                StrokeThickness = 2
            };

            ChartCanvas.Children.Add(polygon);
        }

        private double GetItemValue(object item)
        {
            if (item == null || string.IsNullOrEmpty(ValuePath))
                return 0;

            var property = item.GetType().GetProperty(ValuePath);
            if (property != null)
            {
                var value = property.GetValue(item);
                if (value is double doubleValue)
                {
                    return doubleValue;
                }
                else if (value is int intValue)
                {
                    return intValue;
                }
            }
            return 0;
        }

        private string GetItemLabel(object item)
        {
            if (item == null || string.IsNullOrEmpty(LabelPath))
                return string.Empty;

            var property = item.GetType().GetProperty(LabelPath);
            return property?.GetValue(item)?.ToString() ?? string.Empty;
        }

        private void DrawLegend()
        {
            foreach (var item in ItemsSource)
            {
                string label = GetItemLabel(item);
                var color = GetColorForItem(item);

                var legendItem = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                var colorRect = new Rectangle
                {
                    Width = 16,
                    Height = 16,
                    Fill = new SolidColorBrush(color),
                    Margin = new Thickness(0, 0, 5, 0)
                };

                var labelTextBlock = new TextBlock
                {
                    Text = label,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Colors.White)
                };

                legendItem.Children.Add(colorRect);
                legendItem.Children.Add(labelTextBlock);

                _legendPanel.Children.Add(legendItem);
            }
        }

        private void PositionLegend()
        {
            switch (LegendPosition)
            {
                case LegendPosition.Left:
                    Grid.SetColumn(_legendPanel, 0);
                    Grid.SetRow(_legendPanel, 1);
                    _legendPanel.HorizontalAlignment = HorizontalAlignment.Left;
                    _legendPanel.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case LegendPosition.Top:
                    Grid.SetColumn(_legendPanel, 1);
                    Grid.SetRow(_legendPanel, 0);
                    _legendPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    _legendPanel.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case LegendPosition.Right:
                    Grid.SetColumn(_legendPanel, 2);
                    Grid.SetRow(_legendPanel, 1);
                    _legendPanel.HorizontalAlignment = HorizontalAlignment.Right;
                    _legendPanel.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case LegendPosition.Bottom:
                    Grid.SetColumn(_legendPanel, 1);
                    Grid.SetRow(_legendPanel, 2);
                    _legendPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    _legendPanel.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Loaded += PolarChart_Loaded;
        }

        private void PolarChart_Loaded(object sender, RoutedEventArgs e)
        {
            DrawChart();
            Loaded -= PolarChart_Loaded;
        }

        private Color GetColorForItem(object item)
        {
            return ChartColor;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
