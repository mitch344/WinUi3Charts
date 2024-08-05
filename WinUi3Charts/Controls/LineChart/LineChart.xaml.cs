using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Microsoft.UI.Xaml;

namespace WinUi3Charts.Controls
{
    public sealed partial class LineChart : UserControl
    {


        public LineChart()
        {
            InitializeComponent();
            Loaded += LineChart_Loaded;
            SizeChanged += LineChart_SizeChanged;

            Axis = new Axis()
            {
                ChartHeight = ChartCanvas.ActualHeight,
                ChartWidth = ChartCanvas.ActualWidth,
                AxisColor = AxisColor,
                LabelColor = LabelColor,
                XAxisLabel = XAxisLabel,
                YAxisLabel = YAxisLabel,
                XAxisDateTimeFormat = XAxisDateTimeFormat,
                YAxisDateTimeFormat = YAxisDateTimeFormat
            };

            _legendPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10)
            };

            MainGrid.Children.Add(_legendPanel);
            Grid.SetColumn(_legendPanel, 1);
            Grid.SetRow(_legendPanel, 1);
        }



        private void UpdateChart(bool isInitialLoad = false)
        {
            ChartCanvas.Children.Clear();
            Axis.XAxisDateTimeFormat = XAxisDateTimeFormat;
            Axis.YAxisDateTimeFormat = YAxisDateTimeFormat;

            if (ShowLegend)
            {
                DrawLegend();
                PositionLegend();
            }

            if (SeriesCollection != null && SeriesCollection.Count > 0)
            {
                DrawMultipleSeriesChart(isInitialLoad);
            }
            else if (ChartUtilities.ValidateData(ItemsSource, XValuePath, YValuePath))
            {
                DrawSingleSeriesChart(isInitialLoad);
            }
            else
            {
                ChartUtilities.ShowErrorMessage("Invalid or insufficient data to display the chart.", ChartCanvas);
            }
        }

        private void DrawMultipleSeriesChart(bool isInitialLoad)
        {
            object xMin = null, xMax = null, yMin = null, yMax = null;
            Type xType = null, yType = null;

            foreach (var series in SeriesCollection)
            {
                var data = series.ItemsSource.Cast<object>().ToList();
                var xValues = data.Select(item => ChartUtilities.GetPropertyObjectValue(item, series.XValuePath)).ToList();
                var yValues = data.Select(item => ChartUtilities.GetPropertyObjectValue(item, series.YValuePath)).ToList();

                xType ??= xValues.First()?.GetType();
                yType ??= yValues.First()?.GetType();

                if (xType == typeof(double))
                {
                    xMin = xMin == null ? xValues.Cast<double>().Min() : Math.Min((double)xMin, xValues.Cast<double>().Min());
                    xMax = xMax == null ? xValues.Cast<double>().Max() : Math.Max((double)xMax, xValues.Cast<double>().Max());
                }
                else if (xType == typeof(DateTime))
                {
                    xMin = xMin == null ? xValues.Cast<DateTime>().Min() : ((DateTime)xMin < xValues.Cast<DateTime>().Min() ? (DateTime)xMin : xValues.Cast<DateTime>().Min());
                    xMax = xMax == null ? xValues.Cast<DateTime>().Max() : ((DateTime)xMax > xValues.Cast<DateTime>().Max() ? (DateTime)xMax : xValues.Cast<DateTime>().Max());
                }

                if (yType == typeof(double))
                {
                    yMin = yMin == null ? yValues.Cast<double>().Min() : Math.Min((double)yMin, yValues.Cast<double>().Min());
                    yMax = yMax == null ? yValues.Cast<double>().Max() : Math.Max((double)yMax, yValues.Cast<double>().Max());
                }
                else if (yType == typeof(DateTime))
                {
                    yMin = yMin == null ? yValues.Cast<DateTime>().Min() : ((DateTime)yMin < yValues.Cast<DateTime>().Min() ? (DateTime)yMin : yValues.Cast<DateTime>().Min());
                    yMax = yMax == null ? yValues.Cast<DateTime>().Max() : ((DateTime)yMax > yValues.Cast<DateTime>().Max() ? (DateTime)yMax : yValues.Cast<DateTime>().Max());
                }
            }

            var chartWidth = ChartCanvas.ActualWidth;
            var chartHeight = ChartCanvas.ActualHeight;

            Axis.AxisColor = AxisColor;
            Axis.LabelColor = LabelColor;
            Axis.XAxisLabel = XAxisLabel;
            Axis.YAxisLabel = YAxisLabel;
            Axis.ChartWidth = chartWidth;
            Axis.ChartHeight = chartHeight;

            Axis.DrawXAxis(ChartCanvas, xMin, xMax, xType);
            Axis.DrawYAxis(ChartCanvas, yMin, yMax, yType);

            foreach (var series in SeriesCollection)
            {
                var data = series.ItemsSource.Cast<object>().ToList();
                List<object> xValues, yValues;

                if (xType == typeof(double))
                {
                    xValues = data.Select(item => (object)(double)ChartUtilities.GetPropertyObjectValue(item, series.XValuePath)).ToList();
                }
                else if (xType == typeof(DateTime))
                {
                    xValues = data.Select(item => (object)(DateTime)ChartUtilities.GetPropertyObjectValue(item, series.XValuePath)).ToList();
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported type for X axis: {xType}");
                }

                if (yType == typeof(double))
                {
                    yValues = data.Select(item => (object)(double)ChartUtilities.GetPropertyObjectValue(item, series.YValuePath)).ToList();
                }
                else if (yType == typeof(DateTime))
                {
                    yValues = data.Select(item => (object)(DateTime)ChartUtilities.GetPropertyObjectValue(item, series.YValuePath)).ToList();
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported type for Y axis: {yType}");
                }

                DrawSeries(series, xValues, yValues, xMin, xMax, yMin, yMax, xType, yType, isInitialLoad);
            }
        }

        private void DrawLegend()
        {
            _legendPanel.Children.Clear();

            if (SeriesCollection != null)
            {
                foreach (var series in SeriesCollection)
                {
                    var legendItem = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 0, 0, 5)
                    };

                    var colorRect = new Rectangle
                    {
                        Width = 16,
                        Height = 16,
                        Fill = new SolidColorBrush(series.LineColor),
                        Margin = new Thickness(0, 0, 5, 0)
                    };

                    var labelTextBlock = new TextBlock
                    {
                        Text = series.Name,
                        VerticalAlignment = VerticalAlignment.Center,
                        Foreground = new SolidColorBrush(LabelColor)
                    };

                    legendItem.Children.Add(colorRect);
                    legendItem.Children.Add(labelTextBlock);

                    _legendPanel.Children.Add(legendItem);
                }
            }
            else if (ItemsSource != null)
            {
                var legendItem = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                var colorRect = new Rectangle
                {
                    Width = 16,
                    Height = 16,
                    Fill = new SolidColorBrush(LineColor),
                    Margin = new Thickness(0, 0, 5, 0)
                };

                var labelTextBlock = new TextBlock
                {
                    Text = "Series",
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(LabelColor)
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

        private void DrawSingleSeriesChart(bool isInitialLoad)
        {
            if (!ChartUtilities.ValidateData(ItemsSource, XValuePath, YValuePath))
                return;

            var data = ItemsSource.Cast<object>().ToList();
            var xValues = data.Select(item => ChartUtilities.GetPropertyObjectValue(item, XValuePath)).ToList();
            var yValues = data.Select(item => ChartUtilities.GetPropertyObjectValue(item, YValuePath)).ToList();

            var xType = xValues.First()?.GetType();
            var yType = yValues.First()?.GetType();

            object xMin = null;
            object xMax = null;
            object yMin = null;
            object yMax = null;

            if (xType == typeof(double))
            {
                xMin = XAxisMin ?? (object)xValues.Cast<double>().Min();
                xMax = XAxisMax ?? (object)xValues.Cast<double>().Max();
            }
            else if (xType == typeof(DateTime))
            {
                xMin = XAxisMin ?? (object)xValues.Cast<DateTime>().Min();
                xMax = XAxisMax ?? (object)xValues.Cast<DateTime>().Max();
            }

            if (yType == typeof(double))
            {
                yMin = YAxisMin ?? (object)yValues.Cast<double>().Min();
                yMax = YAxisMax ?? (object)yValues.Cast<double>().Max();
            }
            else if (yType == typeof(DateTime))
            {
                yMin = YAxisMin ?? (object)yValues.Cast<DateTime>().Min();
                yMax = YAxisMax ?? (object)yValues.Cast<DateTime>().Max();
            }

            var chartWidth = ChartCanvas.ActualWidth;
            var chartHeight = ChartCanvas.ActualHeight;

            Axis.AxisColor = AxisColor;
            Axis.LabelColor = LabelColor;
            Axis.XAxisLabel = XAxisLabel;
            Axis.YAxisLabel = YAxisLabel;
            Axis.ChartWidth = chartWidth;
            Axis.ChartHeight = chartHeight;

            Axis.DrawXAxis(ChartCanvas, xMin, xMax, xType);
            Axis.DrawYAxis(ChartCanvas, yMin, yMax, yType);

            var xValuesAsObjects = xValues.Cast<object>().ToList();
            var yValuesAsObjects = yValues.Cast<object>().ToList();

            if (AnimateOnLoad && isInitialLoad)
            {
                DrawAnimatedChart(xValuesAsObjects, yValuesAsObjects, xMin, xMax, yMin, yMax, xType, yType);
            }
            else
            {
                DrawStaticChart(xValuesAsObjects, yValuesAsObjects, xMin, xMax, yMin, yMax, xType, yType);
            }
        }

        private void DrawStaticChart(List<object> xValues, List<object> yValues, object xMin, object xMax, object yMin, object yMax, Type xType, Type yType, LineSeries series = null)
        {
            var lineColor = series?.LineColor ?? LineColor;
            var lineThickness = series?.LineThickness ?? LineThickness;
            var fillColor = series?.FillColor ?? FillColor;
            var fillStyle = series?.FillStyle ?? FillStyle;
            var showDataPoints = series?.ShowDataPoints ?? ShowDataPoints;
            var dataPointSize = series?.DataPointSize ?? DataPointSize;
            var dataPointColor = series?.DataPointColor ?? DataPointColor;

            var line = new Polyline
            {
                Stroke = new SolidColorBrush(lineColor),
                StrokeThickness = lineThickness
            };

            for (var i = 0; i < xValues.Count; i++)
            {
                var x = Axis.ScaleX(xValues[i], xMin, xMax, xType);
                var y = Axis.ScaleY(yValues[i], yMin, yMax, yType);
                line.Points.Add(new Point(x, y));
            }

            ChartCanvas.Children.Add(line);

            if (fillColor.HasValue)
            {
                var fill = new Polygon();
                foreach (var point in line.Points)
                {
                    fill.Points.Add(point);
                }
                fill.Points.Add(new Point(line.Points.Last().X, Axis.ChartHeight - Axis.BottomMargin));
                fill.Points.Add(new Point(line.Points.First().X, Axis.ChartHeight - Axis.BottomMargin));

                if (fillStyle == FillStyle.Flat)
                {
                    fill.Fill = new SolidColorBrush(fillColor.Value);
                }
                else if (fillStyle == FillStyle.Gradient)
                {
                    fill.Fill = CreateGradient3DFill(fillColor.Value, Axis.ChartHeight - Axis.BottomMargin);
                }

                ChartCanvas.Children.Insert(ChartCanvas.Children.Count - 1, fill);
            }

            if (showDataPoints)
            {
                for (var i = 0; i < xValues.Count; i++)
                {
                    var x = Axis.ScaleX(xValues[i], xMin, xMax, xType);
                    var y = Axis.ScaleY(yValues[i], yMin, yMax, yType);
                    var dataPoint = new Ellipse
                    {
                        Fill = new SolidColorBrush(dataPointColor),
                        Width = dataPointSize,
                        Height = dataPointSize
                    };
                    Canvas.SetLeft(dataPoint, x - dataPointSize / 2);
                    Canvas.SetTop(dataPoint, y - dataPointSize / 2);
                    ChartCanvas.Children.Add(dataPoint);
                }
            }
        }

        private void DrawSeries(LineSeries series, List<object> xValues, List<object> yValues, object xMin, object xMax, object yMin, object yMax, Type xType, Type yType, bool animate)
        {
            if (animate && AnimateOnLoad)
            {
                DrawAnimatedChart(xValues, yValues, xMin, xMax, yMin, yMax, xType, yType, series);
            }
            else
            {
                DrawStaticChart(xValues, yValues, xMin, xMax, yMin, yMax, xType, yType, series);
            }
        }

        private Brush CreateGradient3DFill(Color baseColor, double height)
        {
            var gradientStops = new GradientStopCollection();

            Color topColor = ColorHelper.LightenColor(baseColor, 0.3);
            Color bottomColor = ColorHelper.DarkenColor(baseColor, 0.3);

            gradientStops.Add(new GradientStop { Color = topColor, Offset = 0 });
            gradientStops.Add(new GradientStop { Color = baseColor, Offset = 0.5 });
            gradientStops.Add(new GradientStop { Color = bottomColor, Offset = 1 });

            return new LinearGradientBrush
            {
                GradientStops = gradientStops,
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
        }
    }
}
