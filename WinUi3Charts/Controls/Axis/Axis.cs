using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System;
using Windows.UI;
using System.Linq;

namespace WinUi3Charts.Controls
{
    public class Axis
    {
        public Color AxisColor { get; set; }
        public Color LabelColor { get; set; }
        public double ChartWidth { get; set; }
        public double ChartHeight { get; set; }
        public string XAxisLabel { get; set; }
        public string YAxisLabel { get; set; }
        public bool ShowTickLines { get; set; } = true;
        public string XAxisDateTimeFormat { get; set; } = "d MMM";
        public string YAxisDateTimeFormat { get; set; } = "d MMM";
        public double TickLength { get; set; } = 5;
        public double LeftMargin { get; set; } = 40;
        public double BottomMargin { get; set; } = 30;

        public Axis() { }

        public void DrawXAxis(Canvas canvas, object min, object max, Type valueType)
        {
            DrawAxisLine(canvas, LeftMargin, ChartHeight - BottomMargin, ChartWidth, ChartHeight - BottomMargin);
            var steps = CalculateAxisSteps(min, max, ChartWidth - LeftMargin, valueType);
            DrawAxisTicksAndLabels(canvas, steps, min, max, valueType, true);
            DrawAxisLabel(canvas, XAxisLabel, ChartWidth / 2, ChartHeight);
        }


        public void DrawYAxis(Canvas canvas, object min, object max, Type valueType)
        {
            DrawAxisLine(canvas, LeftMargin, 0, LeftMargin, ChartHeight - BottomMargin);
            var steps = CalculateAxisSteps(min, max, ChartHeight - BottomMargin, valueType);
            DrawAxisTicksAndLabels(canvas, steps, min, max, valueType, false);
            DrawAxisLabel(canvas, YAxisLabel, LeftMargin - 60, (ChartHeight - BottomMargin) / 2, -90);
        }

        private void DrawAxisLine(Canvas canvas, double x1, double y1, double x2, double y2)
        {
            var axis = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = new SolidColorBrush(AxisColor),
                StrokeThickness = 1
            };
            canvas.Children.Add(axis);
        }

        private void DrawAxisTicksAndLabels(Canvas canvas, List<object> steps, object min, object max, Type valueType, bool isXAxis)
        {
            foreach (var step in steps)
            {
                double pos = isXAxis ? ScaleX(step, min, max, valueType) : ScaleY(step, min, max, valueType);

                if (pos >= (isXAxis ? LeftMargin : 0) && pos <= (isXAxis ? ChartWidth : ChartHeight - BottomMargin))
                {
                    if (ShowTickLines)
                    {
                        DrawTickLine(canvas, pos, isXAxis);
                    }

                    var label = new TextBlock
                    {
                        Text = FormatAxisLabel(step, valueType, isXAxis),
                        FontSize = 10,
                        Foreground = new SolidColorBrush(LabelColor)
                    };

                    label.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
                    double labelPos = Math.Max((isXAxis ? LeftMargin : 0), Math.Min((isXAxis ? ChartWidth - label.DesiredSize.Width : ChartHeight - BottomMargin - label.DesiredSize.Height), pos - (isXAxis ? label.DesiredSize.Width / 2 : label.DesiredSize.Height / 2)));

                    if (isXAxis)
                    {
                        Canvas.SetLeft(label, labelPos);
                        Canvas.SetTop(label, ChartHeight - BottomMargin + (ShowTickLines ? TickLength : 0) + 2);

                        if (valueType == typeof(DateTime))
                        {
                            label.RenderTransform = new RotateTransform
                            {
                                Angle = 45,
                                CenterX = label.DesiredSize.Width / 2,
                                CenterY = label.DesiredSize.Height / 2
                            };
                        }
                    }
                    else
                    {
                        Canvas.SetLeft(label, LeftMargin - (ShowTickLines ? TickLength : 0) - label.DesiredSize.Width - 10);
                        Canvas.SetTop(label, labelPos);
                    }

                    canvas.Children.Add(label);
                }
            }
        }

        private void DrawTickLine(Canvas canvas, double pos, bool isXAxis)
        {
            var tickLine = new Line
            {
                X1 = isXAxis ? pos : LeftMargin,
                Y1 = isXAxis ? ChartHeight - BottomMargin : pos,
                X2 = isXAxis ? pos : LeftMargin - TickLength,
                Y2 = isXAxis ? ChartHeight - BottomMargin + TickLength : pos,
                Stroke = new SolidColorBrush(AxisColor),
                StrokeThickness = 1
            };
            canvas.Children.Add(tickLine);
        }

        private void DrawAxisLabel(Canvas canvas, string labelText, double left, double top, double angle = 0)
        {
            if (!string.IsNullOrEmpty(labelText))
            {
                var label = new TextBlock
                {
                    Text = labelText,
                    FontSize = 12,
                    TextAlignment = TextAlignment.Center,
                    Foreground = new SolidColorBrush(LabelColor)
                };

                label.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));

                if (angle != 0)
                {
                    var rotateTransform = new RotateTransform
                    {
                        Angle = angle,
                        CenterX = label.DesiredSize.Width / 2,
                        CenterY = label.DesiredSize.Height / 2
                    };
                    label.RenderTransform = rotateTransform;
                }

                Canvas.SetLeft(label, left - label.DesiredSize.Width / 2);
                Canvas.SetTop(label, top);
                canvas.Children.Add(label);
            }
        }

        private List<object> CalculateAxisSteps(object min, object max, double chartSize, Type valueType)
        {
            if (valueType == typeof(double))
            {
                var range = (double)max - (double)min;
                var pixelsPerStep = 30;
                var numberOfSteps = Math.Max(2, Math.Min(10, (int)(chartSize / pixelsPerStep)));

                if (range == 0)
                {
                    return new List<object> { (double)min - 1, min, (double)min + 1 };
                }

                bool isIntegerRange = Math.Abs((double)min - Math.Round((double)min)) < 1e-10 &&
                                      Math.Abs((double)max - Math.Round((double)max)) < 1e-10;

                if (isIntegerRange && range <= 10)
                {
                    return Enumerable.Range((int)Math.Floor((double)min), (int)Math.Ceiling((double)max - (double)min) + 1)
                        .Select(x => (object)(double)x)
                        .ToList();
                }

                var stepSize = NiceNumber(range / (numberOfSteps - 1), true);
                var niceMin = Math.Floor((double)min / stepSize) * stepSize;
                var niceMax = Math.Ceiling((double)max / stepSize) * stepSize;

                var steps = new List<object>();
                for (var step = niceMin; step <= niceMax + stepSize / 2; step += stepSize)
                {
                    steps.Add(step);
                }

                if (steps.Count < 2)
                {
                    steps.Insert(0, niceMin - stepSize);
                    steps.Add(niceMax + stepSize);
                }

                if (steps.All(s => Math.Abs((double)s - Math.Round((double)s)) < 1e-10))
                {
                    return steps.Select(s => (object)Math.Round((double)s)).Distinct().ToList();
                }

                return steps;
            }
            else if (valueType == typeof(DateTime))
            {
                var range = ((DateTime)max - (DateTime)min).TotalDays;
                var pixelsPerStep = 30;
                var numberOfSteps = Math.Max(2, Math.Min(10, (int)(chartSize / pixelsPerStep)));

                var stepSize = NiceTimeSpan(range / (numberOfSteps - 1));
                var niceMin = ((DateTime)min).Date;
                var niceMax = ((DateTime)max).Date;

                var steps = new List<object>();
                for (var step = niceMin; step <= niceMax; step = step.AddDays(stepSize))
                {
                    steps.Add(step);
                }

                if (steps.Count < 2)
                {
                    steps.Insert(0, niceMin.AddDays(-stepSize));
                    steps.Add(niceMax.AddDays(stepSize));
                }

                return steps;
            }
            else if (valueType == typeof(string))
            {
                var range = (int)max - (int)min;
                var pixelsPerStep = 30;
                var numberOfSteps = Math.Max(2, Math.Min(10, (int)(chartSize / pixelsPerStep)));

                if (range == 0)
                {
                    return new List<object> { (int)min - 1, min, (int)min + 1 };
                }

                var stepSize = (range / (numberOfSteps - 1)) + 1;
                var niceMin = (int)min;
                var niceMax = (int)max;

                var steps = new List<object>();
                for (var step = niceMin; step <= niceMax; step += stepSize)
                {
                    steps.Add(step);
                }

                if (steps.Count < 2)
                {
                    steps.Insert(0, niceMin - stepSize);
                    steps.Add(niceMax + stepSize);
                }

                return steps;
            }

            return new List<object>();
        }

        private double NiceNumber(double range, bool round)
        {
            if (range == 0)
                return 1;

            var exponent = Math.Floor(Math.Log10(range));
            var fraction = range / Math.Pow(10, exponent);
            double niceFraction;

            if (round)
            {
                if (fraction < 1.5)
                    niceFraction = 1;
                else if (fraction < 3)
                    niceFraction = 2;
                else if (fraction < 7)
                    niceFraction = 5;
                else
                    niceFraction = 10;
            }
            else
            {
                if (fraction <= 1)
                    niceFraction = 1;
                else if (fraction <= 2)
                    niceFraction = 2;
                else if (fraction <= 5)
                    niceFraction = 5;
                else
                    niceFraction = 10;
            }

            return niceFraction * Math.Pow(10, exponent);
        }

        private int NiceTimeSpan(double range)
        {
            if (range <= 1)
                return 1;
            if (range <= 7)
                return 1;
            if (range <= 30)
                return 7;
            if (range <= 365)
                return 30;
            return 365;
        }

        private string FormatAxisLabel(object value, Type valueType, bool isXAxis)
        {
            if (valueType == typeof(double))
            {
                if (Math.Abs((double)value) < 1e-15)
                {
                    return "0";
                }
                else if (Math.Abs((double)value) < 0.01 || Math.Abs((double)value) >= 1e6)
                {
                    return ((double)value).ToString("E2");
                }
                else if (Math.Abs((double)value - Math.Round((double)value)) < 1e-10)
                {
                    return Math.Round((double)value).ToString("F0");
                }
                else
                {
                    return ((double)value).ToString("G4");
                }
            }
            else if (valueType == typeof(DateTime))
            {
                return ((DateTime)value).ToString(isXAxis ? XAxisDateTimeFormat : YAxisDateTimeFormat);
            }
            else if (valueType == typeof(string))
            {
                return value.ToString();
            }
            return value.ToString();
        }

        public double ScaleX(object x, object min, object max, Type valueType)
        {
            if (valueType == typeof(double))
            {
                return LeftMargin + ((double)x - (double)min) / ((double)max - (double)min) * (ChartWidth - LeftMargin);
            }
            else if (valueType == typeof(DateTime))
            {
                return LeftMargin + (((DateTime)x).Ticks - ((DateTime)min).Ticks) / (double)(((DateTime)max).Ticks - ((DateTime)min).Ticks) * (ChartWidth - LeftMargin);
            }
            else if (valueType == typeof(string))
            {
                return LeftMargin + ((int)x - (int)min) / (double)((int)max - (int)min) * (ChartWidth - LeftMargin);
            }
            return LeftMargin;
        }

        public double ScaleY(object y, object min, object max, Type valueType)
        {
            if (valueType == typeof(double))
            {
                return (ChartHeight - BottomMargin) - ((double)y - (double)min) / ((double)max - (double)min) * (ChartHeight - BottomMargin);
            }
            else if (valueType == typeof(DateTime))
            {
                return (ChartHeight - BottomMargin) - (((DateTime)y).Ticks - ((DateTime)min).Ticks) / (double)(((DateTime)max).Ticks - ((DateTime)min).Ticks) * (ChartHeight - BottomMargin);
            }
            else if (valueType == typeof(string))
            {
                return (ChartHeight - BottomMargin) - ((int)y - (int)min) / (double)((int)max - (int)min) * (ChartHeight - BottomMargin);
            }
            return ChartHeight - BottomMargin;
        }
    }
}
