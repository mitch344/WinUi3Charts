using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;

namespace WinUi3Charts.Controls
{
    public sealed partial class LineChart : UserControl
    {
        private void DrawAnimatedChart(List<object> xValues, List<object> yValues, object xMin, object xMax, object yMin, object yMax, Type xType, Type yType, LineSeries series = null)
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

            var fill = fillColor.HasValue ? new Polygon() : null;
            if (fill != null)
            {
                fill.Fill = fillStyle == FillStyle.Flat
                    ? new SolidColorBrush(fillColor.Value)
                    : CreateGradient3DFill(fillColor.Value, Axis.ChartHeight - Axis.BottomMargin);
                ChartCanvas.Children.Insert(ChartCanvas.Children.Count, fill);
            }

            for (var i = 0; i < xValues.Count; i++)
            {
                var x = Axis.ScaleX(xValues[i], xMin, xMax, xType);
                var y = Axis.ChartHeight - Axis.BottomMargin;
                line.Points.Add(new Point(x, y));

                if (fill != null)
                {
                    fill.Points.Add(new Point(x, y));
                }
            }

            ChartCanvas.Children.Add(line);

            if (fill != null)
            {
                fill.Points.Add(new Point(line.Points.Last().X, Axis.ChartHeight - Axis.BottomMargin));
                fill.Points.Add(new Point(line.Points.First().X, Axis.ChartHeight - Axis.BottomMargin));
            }

            var dataPoints = new List<Ellipse>();
            if (showDataPoints)
            {
                for (var i = 0; i < xValues.Count; i++)
                {
                    var x = Axis.ScaleX(xValues[i], xMin, xMax, xType);
                    var dataPoint = new Ellipse
                    {
                        Fill = new SolidColorBrush(dataPointColor),
                        Width = dataPointSize,
                        Height = dataPointSize
                    };
                    Canvas.SetLeft(dataPoint, x - dataPointSize / 2);
                    Canvas.SetTop(dataPoint, Axis.ChartHeight - Axis.BottomMargin - dataPointSize / 2);
                    ChartCanvas.Children.Add(dataPoint);
                    dataPoints.Add(dataPoint);
                }
            }

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);
            var animationDuration = TimeSpan.FromSeconds(1);
            var elapsedTime = TimeSpan.Zero;

            timer.Tick += (sender, e) =>
            {
                elapsedTime += timer.Interval;
                var progress = Math.Min(elapsedTime.TotalSeconds / animationDuration.TotalSeconds, 1);

                for (var i = 0; i < xValues.Count; i++)
                {
                    var x = Axis.ScaleX(xValues[i], xMin, xMax, xType);
                    var finalY = Axis.ScaleY(yValues[i], yMin, yMax, yType);
                    var currentY = Axis.ChartHeight - Axis.BottomMargin - (Axis.ChartHeight - Axis.BottomMargin - finalY) * progress;

                    if (i < line.Points.Count)
                        line.Points[i] = new Point(x, currentY);

                    if (fill != null && i < fill.Points.Count)
                        fill.Points[i] = new Point(x, currentY);

                    if (showDataPoints && i < dataPoints.Count)
                    {
                        Canvas.SetLeft(dataPoints[i], x - dataPointSize / 2);
                        Canvas.SetTop(dataPoints[i], currentY - dataPointSize / 2);
                    }
                }

                if (fill != null)
                {
                    fill.Points[fill.Points.Count - 2] = new Point(line.Points.Last().X, Axis.ChartHeight - Axis.BottomMargin);
                    fill.Points[fill.Points.Count - 1] = new Point(line.Points.First().X, Axis.ChartHeight - Axis.BottomMargin);
                }

                if (progress >= 1)
                {
                    timer.Stop();
                }
            };

            timer.Start();
        }

        private PointAnimation CreatePointAnimation(Point startPoint, double endY, TimeSpan duration)
        {
            return new PointAnimation
            {
                From = startPoint,
                To = new Point(startPoint.X, endY),
                Duration = duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
        }

        private DoubleAnimation CreateDoubleAnimation(double from, double to, TimeSpan duration)
        {
            return new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
        }
    }
}
