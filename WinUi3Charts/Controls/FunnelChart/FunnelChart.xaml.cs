using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI;

namespace WinUi3Charts.Controls
{
    public partial class FunnelChart : UserControl
    {
        public FunnelChart()
        {
            InitializeComponent();
            SizeChanged += FunnelChart_SizeChanged;
        }

        private void DrawFunnel()
        {
            var items = ItemsSource.Cast<object>().ToList();
            if (ChartCanvas == null || !items.Any())
            {
                ChartUtilities.ShowErrorMessage("No data to display.", ChartCanvas);
                return;
            }

            LeftCanvas.Children.Clear();
            ChartCanvas.Children.Clear();

            double totalValue = items.Sum(item => GetItemValue(item));
            if (totalValue <= 0)
            {
                ChartUtilities.ShowErrorMessage("Invalid data: Total value must be greater than zero.", ChartCanvas);
                return;
            }

            double canvasWidth = ChartCanvas.ActualWidth;
            double canvasHeight = ChartCanvas.ActualHeight;

            if (canvasWidth <= 0 || canvasHeight <= 0)
            {
                return;
            }

            int itemCount = items.Count();
            double sectionHeight = (canvasHeight - (SectionGap * (itemCount - 1))) / itemCount;

            double circlePadding = 10;
            double labelPadding = 10;

            double funnelStartX = CircleSize + circlePadding * 2;
            double funnelWidth = canvasWidth - funnelStartX - labelPadding;

            double startWidth = double.IsNaN(FunnelStartWidth) ? funnelWidth : FunnelStartWidth;
            double endWidth = double.IsNaN(FunnelEndWidth) ? funnelWidth * 0.3 : FunnelEndWidth;

            for (int i = 0; i < itemCount; i++)
            {
                var item = items[i];
                double value = GetItemValue(item);
                double percentage = value / totalValue * 100;
                double width = startWidth + (endWidth - startWidth) * i / (itemCount - 1);
                double yOffset = i * (sectionHeight + SectionGap);
                Point circlePosition = GetCirclePosition(i, sectionHeight + SectionGap, CircleSize);
                Color itemColor = GetItemColor(item);

                if (ShowCircles)
                {
                    DrawPercentageCircle(LeftCanvas, percentage, circlePosition.Y, CircleSize, itemColor);
                }
                DrawFunnelSection(ChartCanvas, yOffset, sectionHeight, width, itemColor, funnelStartX);
                DrawLabelAndValue(ChartCanvas, GetItemLabel(item), value, circlePosition.Y, funnelStartX + width / 2);
            }

            if (ShowCircles)
            {
                DrawConnectingLines(LeftCanvas, sectionHeight + SectionGap, CircleSize / 2);
            }
        }

        private void DrawSphere(Canvas canvas, double percentage, double y, double size, Color color)
        {
            var gradientBrush = new RadialGradientBrush
            {
                Center = new Point(0.5, 0.5),
                RadiusX = 0.5,
                RadiusY = 0.5
            };

            gradientBrush.GradientStops.Add(new GradientStop { Color = ColorHelper.LightenColor(color, 0.1), Offset = 0 });
            gradientBrush.GradientStops.Add(new GradientStop { Color = color, Offset = 0.8 });
            gradientBrush.GradientStops.Add(new GradientStop { Color = ColorHelper.DarkenColor(color, 0.1), Offset = 1 });

            var sphere = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = gradientBrush
            };

            Canvas.SetLeft(sphere, 0);
            Canvas.SetTop(sphere, y - size / 2);
            canvas.Children.Add(sphere);

            var text = new TextBlock
            {
                Text = $"{percentage:F1}%",
                Style = GetStyle("DefaultCircleStyle", CircleStyle)
            };

            text.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            Canvas.SetLeft(text, (size - text.DesiredSize.Width) / 2);
            Canvas.SetTop(text, y - text.DesiredSize.Height / 2);
            canvas.Children.Add(text);
        }

        private void DrawPercentageCircle(Canvas canvas, double percentage, double y, double size, Color color)
        {
            if (Use3DEffect && UseSphereForCircles)
            {
                DrawSphere(canvas, percentage, y, size, color);
            }
            else
            {
                var ellipse = new Ellipse
                {
                    Width = size,
                    Height = size,
                    Fill = new SolidColorBrush(color)
                };
                Canvas.SetLeft(ellipse, 0);
                Canvas.SetTop(ellipse, y - size / 2);
                canvas.Children.Add(ellipse);

                var text = new TextBlock
                {
                    Text = $"{percentage:F1}%",
                    Style = GetStyle("DefaultCircleStyle", CircleStyle)
                };

                text.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                Canvas.SetLeft(text, (size - text.DesiredSize.Width) / 2);
                Canvas.SetTop(text, y - text.DesiredSize.Height / 2);
                canvas.Children.Add(text);
            }
        }

        private void DrawFunnelSection(Canvas canvas, double y, double height, double width, Color color, double startX)
        {
            if (Use3DEffect)
            {
                DrawPrism(canvas, y, height, width, color, startX);
            }
            else
            {
                double rectHeight = Math.Max(1, height - 10);
                double rectWidth = Math.Max(1, width);

                var rectangle = new Rectangle
                {
                    Height = rectHeight,
                    Width = rectWidth,
                    RadiusX = 10,
                    RadiusY = 10,
                    Fill = CreateGradientBrush(color)
                };

                Canvas.SetLeft(rectangle, startX);
                Canvas.SetTop(rectangle, y + 5);
                canvas.Children.Add(rectangle);
            }
        }

        private void DrawPrism(Canvas canvas, double y, double height, double width, Color color, double startX)
        {
            double rectHeight = Math.Max(1, height - 10);
            double rectWidth = Math.Max(1, width);

            // Front face
            var frontRect = new Rectangle
            {
                Height = rectHeight,
                Width = rectWidth,
                Fill = CreateGradientBrush(color)
            };

            Canvas.SetLeft(frontRect, startX);
            Canvas.SetTop(frontRect, y + 5);
            canvas.Children.Add(frontRect);

            // Top face
            var topPath = new Path
            {
                Fill = new SolidColorBrush(ColorHelper.LightenColor(color, 0.1)),
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                        new PathFigure
                        {
                            StartPoint = new Point(startX, y + 5),
                            Segments = new PathSegmentCollection
                            {
                                new LineSegment { Point = new Point(startX + rectWidth, y + 5) },
                                new LineSegment { Point = new Point(startX + rectWidth + Depth3D, y + 5 - Depth3D) },
                                new LineSegment { Point = new Point(startX + Depth3D, y + 5 - Depth3D) },
                                new LineSegment { Point = new Point(startX, y + 5) }
                            }
                        }
                    }
                }
            };
            canvas.Children.Add(topPath);

            // Right face
            var rightPath = new Path
            {
                Fill = new SolidColorBrush(ColorHelper.LightenColor(color, 0.1)),
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                        new PathFigure
                        {
                            StartPoint = new Point(startX + rectWidth, y + 5),
                            Segments = new PathSegmentCollection
                            {
                                new LineSegment { Point = new Point(startX + rectWidth, y + 5 + rectHeight) },
                                new LineSegment { Point = new Point(startX + rectWidth + Depth3D, y + 5 + rectHeight - Depth3D) },
                                new LineSegment { Point = new Point(startX + rectWidth + Depth3D, y + 5 - Depth3D) },
                                new LineSegment { Point = new Point(startX + rectWidth, y + 5) }
                            }
                        }
                    }
                }
            };
            canvas.Children.Add(rightPath);
        }

        private void DrawLabelAndValue(Canvas canvas, string label, double value, double y, double x)
        {
            var labelText = new TextBlock
            {
                Text = label,
                Style = GetStyle("DefaultLabelStyle", LabelStyle)
            };

            var valueText = new TextBlock
            {
                Text = $"{value:N0}",
                Style = GetStyle("DefaultValueStyle", ValueStyle)
            };

            labelText.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            valueText.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            Canvas.SetLeft(labelText, x - labelText.DesiredSize.Width / 2);
            Canvas.SetTop(labelText, y - labelText.DesiredSize.Height - valueText.DesiredSize.Height / 2);
            canvas.Children.Add(labelText);

            Canvas.SetLeft(valueText, x - valueText.DesiredSize.Width / 2);
            Canvas.SetTop(valueText, y + labelText.DesiredSize.Height / 2);
            canvas.Children.Add(valueText);
        }

        private void DrawConnectingLines(Canvas canvas, double sectionHeight, double circleRadius)
        {
            int itemCount = ItemsSource.Cast<object>().Count();
            for (int i = 1; i < itemCount; i++)
            {
                var line = new Line
                {
                    X1 = circleRadius,
                    Y1 = (i - 1) * sectionHeight + sectionHeight / 2 + circleRadius,
                    X2 = circleRadius,
                    Y2 = i * sectionHeight + sectionHeight / 2 - circleRadius,
                    Stroke = new SolidColorBrush(ConnectingLineColor),
                    StrokeThickness = ConnectingLineThickness
                };
                canvas.Children.Add(line);
            }
        }

        private LinearGradientBrush CreateGradientBrush(Color baseColor)
        {
            return new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop { Color = baseColor, Offset = 0 },
                    new GradientStop { Color = ColorHelper.LightenColor(baseColor, 0.5), Offset = 1 }
                }
            };
        }

        private new void UpdateLayout()
        {
            MainGrid.ColumnDefinitions[0].Width = ShowCircles
                ? new GridLength(CircleSize + 20)
                : new GridLength(0);
        }

        private string GetItemLabel(object item)
        {
            return item?.GetType().GetProperty(LabelPath)?.GetValue(item)?.ToString() ?? string.Empty;
        }

        private double GetItemValue(object item)
        {
            var value = item?.GetType().GetProperty(ValuePath)?.GetValue(item);
            return value != null ? Convert.ToDouble(value) : 0;
        }

        private Color GetItemColor(object item)
        {
            var colorValue = item?.GetType().GetProperty(ColorPath)?.GetValue(item);
            return colorValue is Color color ? color : Colors.Gray;
        }

        private Point GetCirclePosition(int index, double sectionHeight, double circleSize)
        {
            double y = index * sectionHeight + sectionHeight / 2;
            return new Point(circleSize / 2, y);
        }

        private Style GetStyle(string defaultStyleKey, Style customStyle)
        {
            return customStyle ?? (Style)Resources[defaultStyleKey];
        }
    }
}