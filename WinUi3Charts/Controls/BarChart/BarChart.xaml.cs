using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;

namespace WinUi3Charts.Controls
{
    public sealed partial class BarChart : UserControl
    {
        public BarChart()
        {
            this.InitializeComponent();
            this.SizeChanged += BarChart_SizeChanged;
        }

        private void UpdateChart()
        {
            ChartCanvas.Children.Clear();

            if (!ChartUtilities.ValidateData(ItemsSource, ValuePath, LabelPath))
            {
                DisplayErrorMessage("Invalid or insufficient data to display the chart.");
                return;
            }

            var data = ItemsSource.Cast<object>().ToList();
            var (maxValue, minValue) = GetValueRange(data);

            var chartDimensions = GetChartDimensions();
            var axisDimensions = CalculateAxisDimensions(chartDimensions, data);

            int barCount = CalculateBarCount(axisDimensions);
            var barDimensions = CalculateBarDimensions(axisDimensions, barCount);

            DrawBars(data, barCount, axisDimensions, barDimensions, minValue, maxValue);
        }

        private void DisplayErrorMessage(string message)
        {
            var errorMessage = new TextBlock
            {
                Text = message,
                Foreground = new SolidColorBrush(Colors.Red),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            ChartCanvas.Children.Add(errorMessage);
        }

        private (double max, double min) GetValueRange(List<object> data)
        {
            var maxValue = data.Max(item => ChartUtilities.GetPropertyValue(item, ValuePath));
            var minValue = data.Min(item => ChartUtilities.GetPropertyValue(item, ValuePath));
            return (maxValue, minValue);
        }

        private (double width, double height) GetChartDimensions()
        {
            return (ChartCanvas.ActualWidth, ChartCanvas.ActualHeight);
        }

        private (double x, double y, double width, double height) CalculateAxisDimensions((double width, double height) chartDimensions, List<object> data)
        {
            double maxLabelWidth = data.Max(item => MeasureTextWidth(ChartUtilities.GetPropertyStringValue(item, LabelPath)));
            double maxLabelHeight = MeasureTextHeight("Test");
            double maxValueWidth = MeasureTextWidth(data.Max(item => ChartUtilities.GetPropertyValue(item, ValuePath)).ToString("F2"));

            const double labelPadding = 5;

            if (Orientation == Orientation.Vertical)
            {
                double axisX = Math.Max(maxValueWidth, 30) + labelPadding;
                double axisY = labelPadding;
                double axisWidth = chartDimensions.width - axisX - labelPadding;
                double axisHeight = chartDimensions.height - axisY - labelPadding - maxLabelHeight - labelPadding;
                return (axisX, axisY, axisWidth, axisHeight);
            }
            else
            {
                double axisX = maxLabelWidth + labelPadding;
                double axisY = labelPadding;
                double axisWidth = chartDimensions.width - axisX - labelPadding - maxValueWidth;
                double axisHeight = chartDimensions.height - 2 * labelPadding;
                return (axisX, axisY, axisWidth, axisHeight);
            }
        }

        private int CalculateBarCount((double x, double y, double width, double height) axisDimensions)
        {
            const double minBarThickness = 20;
            int maxBars = (int)Math.Floor(Orientation == Orientation.Vertical ? axisDimensions.width / minBarThickness : axisDimensions.height / minBarThickness);
            return Math.Min(ItemsSource.Cast<object>().Count(), maxBars);
        }

        private (double thickness, double spacing) CalculateBarDimensions((double x, double y, double width, double height) axisDimensions, int barCount)
        {
            double totalBarSpace = Orientation == Orientation.Vertical ? axisDimensions.width : axisDimensions.height;
            double barThickness = totalBarSpace / barCount;
            double barSpacing = barThickness * 0.2;
            barThickness -= barSpacing;
            return (barThickness, barSpacing);
        }

        private void DrawBars(List<object> data, int barCount, (double x, double y, double width, double height) axisDimensions, (double thickness, double spacing) barDimensions, double minValue, double maxValue)
        {
            for (int i = 0; i < barCount; i++)
            {
                var item = data[i];
                double value = ChartUtilities.GetPropertyValue(item, ValuePath);
                double normalizedValue = (value - minValue) / (maxValue - minValue);

                DrawBar(i, normalizedValue, axisDimensions, barDimensions, item);
                DrawLabel(i, item, axisDimensions, barDimensions);
                DrawValueLabel(i, value, normalizedValue, axisDimensions, barDimensions);
            }
        }

        private void DrawBar(int index, double normalizedValue, (double x, double y, double width, double height) axisDimensions, (double thickness, double spacing) barDimensions, object item)
        {
            double barLength = normalizedValue * (Orientation == Orientation.Vertical ? axisDimensions.height : axisDimensions.width);

            Color barColor = BarColor;

            if (!string.IsNullOrEmpty(IndividualBarColorPath))
            {
                var customColor = ChartUtilities.GetPropertyObjectValue(item, IndividualBarColorPath) as Color?;
                if (customColor.HasValue)
                {
                    barColor = customColor.Value;
                }
            }

            switch (BarStyle)
            {
                case BarStyle.Flat:
                    DrawFlatBar(index, barLength, axisDimensions, barDimensions, barColor);
                    break;
                case BarStyle.Gradient3D:
                    DrawGradient3DBar(index, barLength, axisDimensions, barDimensions, barColor);
                    break;
                case BarStyle.Prism3D:
                    DrawPrismBar(index, barLength, axisDimensions, barDimensions, barColor);
                    break;
            }
        }

        private void DrawLabel(int index, object item, (double x, double y, double width, double height) axisDimensions, (double thickness, double spacing) barDimensions)
        {
            var label = new TextBlock
            {
                Text = ChartUtilities.GetPropertyStringValue(item, LabelPath),
                Style = LabelStyle ?? (Style)Resources["DefaultLabelStyle"]
            };

            if (Orientation == Orientation.Vertical)
            {
                label.Text = TruncateWithEllipsis(label.Text, barDimensions.thickness);
                label.TextAlignment = TextAlignment.Center;
                label.TextWrapping = TextWrapping.NoWrap;
                label.TextTrimming = TextTrimming.CharacterEllipsis;
                label.Width = barDimensions.thickness;
                Canvas.SetLeft(label, axisDimensions.x + index * (barDimensions.thickness + barDimensions.spacing));
                Canvas.SetTop(label, axisDimensions.y + axisDimensions.height + 5);
            }
            else
            {
                Canvas.SetLeft(label, 5);
                Canvas.SetTop(label, axisDimensions.y + index * (barDimensions.thickness + barDimensions.spacing) + (barDimensions.thickness / 2) - (label.ActualHeight / 2));
            }

            ChartCanvas.Children.Add(label);
        }

        private void DrawValueLabel(int index, double value, double normalizedValue, (double x, double y, double width, double height) axisDimensions, (double thickness, double spacing) barDimensions)
        {
            var valueLabel = new TextBlock
            {
                Text = value.ToString("F2"),
                Style = ValueLabelStyle ?? (Style)Resources["DefaultValueLabelStyle"]
            };

            valueLabel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double labelWidth = valueLabel.DesiredSize.Width;
            double labelHeight = valueLabel.DesiredSize.Height;

            if (Orientation == Orientation.Vertical)
            {
                double barLength = normalizedValue * axisDimensions.height;
                double labelX = axisDimensions.x + index * (barDimensions.thickness + barDimensions.spacing) + (barDimensions.thickness / 2) - (labelWidth / 2);
                double labelY = axisDimensions.y + axisDimensions.height - barLength - labelHeight - 5;

                Canvas.SetLeft(valueLabel, labelX);
                Canvas.SetTop(valueLabel, labelY);
            }
            else
            {
                double barLength = normalizedValue * axisDimensions.width;
                double labelX = axisDimensions.x + barLength + 5;
                double labelY = axisDimensions.y + index * (barDimensions.thickness + barDimensions.spacing) + (barDimensions.thickness / 2) - (labelHeight / 2);

                Canvas.SetLeft(valueLabel, labelX);
                Canvas.SetTop(valueLabel, labelY);
            }

            ChartCanvas.Children.Add(valueLabel);
        }

        private void DrawFlatBar(int index, double barLength, (double x, double y, double width, double height) axisDimensions, (double thickness, double spacing) barDimensions, Color barColor)
        {
            var bar = new Rectangle
            {
                Fill = new SolidColorBrush(barColor),
                Width = Orientation == Orientation.Vertical ? barDimensions.thickness : barLength,
                Height = Orientation == Orientation.Vertical ? barLength : barDimensions.thickness
            };

            SetBarPosition(index, bar, axisDimensions, barDimensions, barLength);
            ChartCanvas.Children.Add(bar);
        }

        private void DrawGradient3DBar(int index, double barLength, (double x, double y, double width, double height) axisDimensions, (double thickness, double spacing) barDimensions, Color barColor)
        {
            LinearGradientBrush gradientBrush = Create3DGradientBrush(barColor, Orientation);

            var bar = new Rectangle
            {
                Fill = gradientBrush,
                Width = Orientation == Orientation.Vertical ? barDimensions.thickness : barLength,
                Height = Orientation == Orientation.Vertical ? barLength : barDimensions.thickness
            };

            SetBarPosition(index, bar, axisDimensions, barDimensions, barLength);
            ChartCanvas.Children.Add(bar);
        }

        private void DrawPrismBar(int index, double barLength, (double x, double y, double width, double height) axisDimensions, (double thickness, double spacing) barDimensions, Color barColor)
        {
            double depthFactor = 0.2;
            double depth = barDimensions.thickness * depthFactor;

            if (Orientation == Orientation.Vertical)
            {
                DrawPrismBarVertical(index, barLength, axisDimensions, barDimensions, barColor, depth);
            }
            else
            {
                DrawPrismBarHorizontal(index, barLength, axisDimensions, barDimensions, barColor, depth);
            }
        }

        private void DrawPrismBarVertical(int index, double barLength, (double x, double y, double width, double height) axisDimensions, (double thickness, double spacing) barDimensions, Color barColor, double depth)
        {
            double left = axisDimensions.x + index * (barDimensions.thickness + barDimensions.spacing);
            double top = axisDimensions.y + axisDimensions.height - barLength;

            // Front face
            var frontRect = CreateRectangle(barColor, barDimensions.thickness, barLength);
            SetPosition(frontRect, left, top);
            ChartCanvas.Children.Add(frontRect);

            // Top face
            var topPath = CreatePrismPath(ColorHelper.LightenColor(barColor, 0.2), left, top, barDimensions.thickness, depth, true);
            ChartCanvas.Children.Add(topPath);

            // Right face
            var rightPath = CreatePrismPath(ColorHelper.DarkenColor(barColor, 0.2), left + barDimensions.thickness, top, barLength, depth, false);
            ChartCanvas.Children.Add(rightPath);
        }

        private void DrawPrismBarHorizontal(int index, double barLength, (double x, double y, double width, double height) axisDimensions, (double thickness, double spacing) barDimensions, Color barColor, double depth)
        {
            double left = axisDimensions.x;
            double top = axisDimensions.y + index * (barDimensions.thickness + barDimensions.spacing);

            // Front face
            var frontRect = CreateRectangle(barColor, barLength, barDimensions.thickness);
            SetPosition(frontRect, left, top);
            ChartCanvas.Children.Add(frontRect);

            // Top face
            var topPath = CreatePrismPath(ColorHelper.LightenColor(barColor, 0.2), left, top, barLength, depth, true);
            ChartCanvas.Children.Add(topPath);

            // Right face
            var rightPath = CreatePrismPath(ColorHelper.DarkenColor(barColor, 0.2), left + barLength, top, barDimensions.thickness, depth, false);
            ChartCanvas.Children.Add(rightPath);
        }

        private Rectangle CreateRectangle(Color color, double width, double height)
        {
            return new Rectangle
            {
                Fill = new SolidColorBrush(color),
                Width = width,
                Height = height
            };
        }

        private void SetPosition(UIElement element, double left, double top)
        {
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
        }

        private Path CreatePrismPath(Color color, double left, double top, double length, double depth, bool isTop)
        {
            var path = new Path
            {
                Fill = new SolidColorBrush(color),
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                        new PathFigure
                        {
                            StartPoint = new Point(left, top),
                            Segments = new PathSegmentCollection
                            {
                                new LineSegment { Point = new Point(left + length, top) },
                                new LineSegment { Point = new Point(left + length + depth, top - (isTop ? depth : -depth)) },
                                new LineSegment { Point = new Point(left + depth, top - (isTop ? depth : -depth)) }
                            }
                        }
                    }
                }
            };
            return path;
        }

        private void SetBarPosition(int index, Rectangle bar, (double x, double y, double width, double height) axisDimensions, (double thickness, double spacing) barDimensions, double barLength)
        {
            if (Orientation == Orientation.Vertical)
            {
                Canvas.SetLeft(bar, axisDimensions.x + index * (barDimensions.thickness + barDimensions.spacing));
                Canvas.SetTop(bar, axisDimensions.y + axisDimensions.height - barLength);
            }
            else
            {
                Canvas.SetLeft(bar, axisDimensions.x);
                Canvas.SetTop(bar, axisDimensions.y + index * (barDimensions.thickness + barDimensions.spacing));
            }
        }

        private LinearGradientBrush Create3DGradientBrush(Color baseColor, Orientation orientation)
        {
            var gradientStops = new GradientStopCollection
            {
                new GradientStop { Color = ColorHelper.LightenColor(baseColor, 0.3), Offset = 0 },
                new GradientStop { Color = baseColor, Offset = 0.3 },
                new GradientStop { Color = baseColor, Offset = 0.7 },
                new GradientStop { Color = ColorHelper.DarkenColor(baseColor, 0.3), Offset = 1 }
            };

            return new LinearGradientBrush(gradientStops, orientation == Orientation.Vertical ? 90 : 0);
        }

        private string TruncateWithEllipsis(string text, double maxWidth)
        {
            var textBlock = new TextBlock { Text = text };
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            if (textBlock.DesiredSize.Width <= maxWidth)
                return text;

            while (textBlock.DesiredSize.Width > maxWidth && text.Length > 3)
            {
                text = text.Substring(0, text.Length - 1);
                textBlock.Text = text + "...";
                textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            return textBlock.Text;
        }

        private double MeasureTextWidth(string text)
        {
            var textBlock = new TextBlock { Text = text };
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return textBlock.DesiredSize.Width;
        }

        private double MeasureTextHeight(string text)
        {
            var textBlock = new TextBlock { Text = text };
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return textBlock.DesiredSize.Height;
        }
    }
}
