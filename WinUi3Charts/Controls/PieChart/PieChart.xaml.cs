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
    public sealed partial class PieChart : UserControl
    {
        public PieChart()
        {
            this.InitializeComponent();
            this.SizeChanged += PieChart_SizeChanged;
            this.Loaded += PieChart_Loaded;
        }

        private Point GetPoint(Point center, double radius, double angle)
        {
            double radians = angle * Math.PI / 180;
            return new Point(center.X + radius * Math.Cos(radians), center.Y + radius * Math.Sin(radians));
        }

        private Color AdjustBrightness(Color color, double factor)
        {
            return Color.FromArgb(
                color.A,
                (byte)Math.Min(255, color.R * factor),
                (byte)Math.Min(255, color.G * factor),
                (byte)Math.Min(255, color.B * factor)
            );
        }

        private Color GetSliceColor(object item, int index)
        {
            if (!string.IsNullOrEmpty(SliceColorPath))
            {
                var property = item.GetType().GetProperty(SliceColorPath);
                if (property != null && property.GetValue(item) is Color color)
                {
                    return color;
                }
            }

            var defaultColors = new List<Color>
            {
                Colors.Red,
                Colors.Blue,
                Colors.Green,
                Colors.Yellow,
                Colors.Purple,
                Colors.Orange,
                Colors.Cyan,
                Colors.Magenta
            };

            return defaultColors[index % defaultColors.Count];
        }

        private double GetSliceAngle(Path slice)
        {
            var geometry = slice.Data as PathGeometry;
            var figure = geometry.Figures[0];
            var arc = figure.Segments[1] as ArcSegment;
            return Math.Atan2(arc.Point.Y - ActualHeight / 2, arc.Point.X - ActualWidth / 2);
        }

        private void UpdateChart(bool isInitialLoad = false)
        {
            ChartCanvas.Children.Clear();
            if (!ChartUtilities.ValidateData(ItemsSource, ValuePath, LabelPath))
            {
                ChartUtilities.ShowErrorMessage("Invalid or insufficient data to display the chart.", ChartCanvas);
                return;
            }

            if (AnimateOnLoad && isInitialLoad)
            {
                _ = DrawAnimatedChart();
            }
            else
            {
                DrawChart(1.0, 0);
            }
        }

        private void DrawChart(double sweepProgress, double innerRadiusRatio)
        {
            ChartCanvas.Children.Clear();
            if (!ChartUtilities.ValidateData(ItemsSource, ValuePath, LabelPath))
                return;

            var data = ItemsSource.Cast<object>().ToList();
            var total = data.Sum(item => ChartUtilities.GetPropertyValue(item, ValuePath));

            Size legendSize = CalculateLegendSize(data);

            double availableWidth = Math.Max(ActualWidth, 1);
            double availableHeight = Math.Max(ActualHeight, 1);

            if (ShowLegend)
            {
                switch (LegendPosition)
                {
                    case LegendPosition.Left:
                    case LegendPosition.Right:
                        availableWidth = Math.Max(availableWidth - legendSize.Width - 20, 1);
                        break;
                    case LegendPosition.Top:
                    case LegendPosition.Bottom:
                        availableHeight = Math.Max(availableHeight - legendSize.Height - 20, 1);
                        break;
                }
            }

            double radius = Math.Max(Math.Min(availableWidth, availableHeight) / 2 * 0.8, 1);
            Point center = new Point(availableWidth / 2, availableHeight / 2);

            if (ShowLegend)
            {
                switch (LegendPosition)
                {
                    case LegendPosition.Left:
                        center.X += legendSize.Width / 2 + 10;
                        break;
                    case LegendPosition.Top:
                        center.Y += legendSize.Height / 2 + 10;
                        break;
                }
            }

            double startAngle = StartAngle;

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                double value = ChartUtilities.GetPropertyValue(item, ValuePath);
                double sweepAngle = 360 * (value / total);

                Color sliceColor = GetSliceColor(item, i);

                DrawSlice(center, radius, startAngle, sweepAngle, sliceColor, item, i, sweepProgress, innerRadiusRatio);

                if (ShowLabels || ShowValues)
                {
                    string label = ShowLabels ? ChartUtilities.GetPropertyStringValue(item, LabelPath) : "";
                    string valueText = ShowValues ? value.ToString("F1") : "";
                    DrawLabelAndValue(center, radius, startAngle, sweepAngle, label, valueText);
                }

                startAngle += sweepAngle + SliceSpacing;
            }

            if (ShowLegend)
                DrawLegend(data);
        }

        private void DrawSlice(Point center, double radius, double startAngle, double sweepAngle, Color color, object dataItem, int index, double sweepProgress = 1.0, double innerRadiusRatio = 0)
        {
            if (sweepAngle < 0.1)
                return;

            var slice = new Path
            {
                Fill = new SolidColorBrush(color),
                Data = CreateSliceGeometry(center, radius, startAngle, sweepAngle * sweepProgress, innerRadiusRatio),
                Tag = index
            };

            if (SliceStyle == SliceStyle.Gradient)
            {
                slice.Fill = CreateGradientBrush(color);
            }
            else if (SliceStyle == SliceStyle.Specular)
            {
                ApplySpecularEffect(slice, color);
            }

            if (index == SelectedSliceIndex)
            {
                HighlightSlice(slice);
            }

            slice.PointerPressed += Slice_PointerPressed;

            ChartCanvas.Children.Add(slice);
        }

        private Geometry CreateSliceGeometry(Point center, double radius, double startAngle, double sweepAngle, double innerRadiusRatio)
        {
            Point startPoint = GetPoint(center, radius, startAngle);
            Point endPoint = GetPoint(center, radius, startAngle + sweepAngle);

            Point innerStartPoint = GetPoint(center, radius * innerRadiusRatio, startAngle);
            Point innerEndPoint = GetPoint(center, radius * innerRadiusRatio, startAngle + sweepAngle);

            var figure = new PathFigure
            {
                StartPoint = innerStartPoint,
                Segments = new PathSegmentCollection
                {
                    new LineSegment { Point = startPoint },
                    new ArcSegment
                    {
                        Point = endPoint,
                        Size = new Size(radius, radius),
                        IsLargeArc = sweepAngle > 180,
                        SweepDirection = SweepDirection.Clockwise
                    },
                    new LineSegment { Point = innerEndPoint },
                    new ArcSegment
                    {
                        Point = innerStartPoint,
                        Size = new Size(radius * innerRadiusRatio, radius * innerRadiusRatio),
                        IsLargeArc = sweepAngle > 180,
                        SweepDirection = SweepDirection.Counterclockwise
                    }
                }
            };

            return new PathGeometry { Figures = new PathFigureCollection { figure } };
        }

        private LinearGradientBrush CreateGradientBrush(Color baseColor)
        {
            return new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop { Color = baseColor, Offset = 0 },
                    new GradientStop { Color = AdjustBrightness(baseColor, 0.7), Offset = 1 }
                }
            };
        }

        private void ApplySpecularEffect(Path slice, Color color)
        {
            var gradientBrush = new RadialGradientBrush
            {
                GradientOrigin = new Point(0.5, 0.5),
                Center = new Point(0.5, 0.5),
                RadiusX = 0.5,
                RadiusY = 0.5
            };

            gradientBrush.GradientStops.Add(new GradientStop { Color = ColorHelper.LightenColor(color, 0.1), Offset = 0 });
            gradientBrush.GradientStops.Add(new GradientStop { Color = color, Offset = 0.5 });
            gradientBrush.GradientStops.Add(new GradientStop { Color = ColorHelper.DarkenColor(color, 0.1), Offset = 1 });

            slice.Fill = gradientBrush;

            slice.RenderTransform = new ScaleTransform
            {
                ScaleX = 1.05,
                ScaleY = 1.05,
                CenterX = ActualWidth / 2,
                CenterY = ActualHeight / 2
            };
        }


        private void HighlightSlice(Path slice)
        {
            var transform = new CompositeTransform();
            var centerX = ActualWidth / 2;
            var centerY = ActualHeight / 2;
            var angle = GetSliceAngle(slice);
            transform.TranslateX = Math.Cos(angle) * 10;
            transform.TranslateY = Math.Sin(angle) * 10;
            slice.RenderTransform = transform;

            var glowSlice = new Path
            {
                Data = slice.Data,
                Fill = new SolidColorBrush(Colors.White),
                Opacity = 0.5,
                RenderTransform = new ScaleTransform { ScaleX = 1.1, ScaleY = 1.1 }
            };

            var index = ChartCanvas.Children.IndexOf(slice);
            ChartCanvas.Children.Insert(index, glowSlice);
        }

        private Size CalculateLegendSize(List<object> data)
        {
            if (!ShowLegend)
                return new Size(0, 0);

            var legend = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                var legendItem = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                var colorRect = new Rectangle
                {
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(0, 0, 5, 0)
                };

                var label = new TextBlock
                {
                    Text = ChartUtilities.GetPropertyStringValue(item, LabelPath),
                    Style = LegendItemStyle ?? (Style)Resources["DefaultLegendItemStyle"]
                };

                legendItem.Children.Add(colorRect);
                legendItem.Children.Add(label);
                legend.Children.Add(legendItem);
            }

            legend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return legend.DesiredSize;
        }

        private void DrawLabelAndValue(Point center, double radius, double startAngle, double sweepAngle, string label, string value)
        {
            double midAngle = startAngle + sweepAngle / 2;
            Point labelPoint = GetPoint(center, radius * 1.1, midAngle);

            var textBlock = new TextBlock
            {
                Text = $"{label}\n{value}",
                Style = LabelStyle ?? (Style)Resources["DefaultLabelStyle"]
            };

            Canvas.SetLeft(textBlock, labelPoint.X - textBlock.ActualWidth / 2);
            Canvas.SetTop(textBlock, labelPoint.Y - textBlock.ActualHeight / 2);

            ChartCanvas.Children.Add(textBlock);
        }

        private void DrawLegend(List<object> data)
        {
            if (!ShowLegend)
                return;

            var legend = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                var legendItem = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                Color itemColor = GetSliceColor(item, i);

                var colorRect = new Rectangle
                {
                    Width = 20,
                    Height = 20,
                    Fill = new SolidColorBrush(itemColor),
                    Margin = new Thickness(0, 0, 5, 0)
                };

                var label = new TextBlock
                {
                    Text = ChartUtilities.GetPropertyStringValue(item, LabelPath),
                    Style = LegendItemStyle ?? (Style)Resources["DefaultLegendItemStyle"]
                };

                legendItem.Children.Add(colorRect);
                legendItem.Children.Add(label);
                legend.Children.Add(legendItem);
            }

            Size legendSize = CalculateLegendSize(data);

            switch (LegendPosition)
            {
                case LegendPosition.Left:
                    Canvas.SetLeft(legend, 10);
                    Canvas.SetTop(legend, (ActualHeight - legendSize.Height) / 2);
                    break;
                case LegendPosition.Right:
                    Canvas.SetLeft(legend, ActualWidth - legendSize.Width - 10);
                    Canvas.SetTop(legend, (ActualHeight - legendSize.Height) / 2);
                    break;
                case LegendPosition.Top:
                    Canvas.SetTop(legend, 10);
                    Canvas.SetLeft(legend, (ActualWidth - legendSize.Width) / 2);
                    break;
                case LegendPosition.Bottom:
                    Canvas.SetTop(legend, ActualHeight - legendSize.Height - 10);
                    Canvas.SetLeft(legend, (ActualWidth - legendSize.Width) / 2);
                    break;
            }

            ChartCanvas.Children.Add(legend);
        }

        private void UpdateSelection()
        {
            for (int i = ChartCanvas.Children.Count - 1; i >= 0; i--)
            {
                if (ChartCanvas.Children[i] is Path slice)
                {
                    if ((int)slice.Tag == SelectedSliceIndex)
                    {
                        HighlightSlice(slice);
                    }
                    else
                    {
                        slice.RenderTransform = null;

                        if (i > 0 && ChartCanvas.Children[i - 1] is Path glowSlice && glowSlice.Opacity == 0.5)
                        {
                            ChartCanvas.Children.RemoveAt(i - 1);
                        }
                    }
                }
            }
        }
    }
}
