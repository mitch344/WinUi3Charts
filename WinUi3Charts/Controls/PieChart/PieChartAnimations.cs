using Microsoft.UI.Xaml.Controls;
using System.Linq;
using System.Threading.Tasks;
using System;
using Windows.Foundation;
using Windows.UI;

namespace WinUi3Charts.Controls
{
    public sealed partial class PieChart : UserControl
    {
        private async Task DrawAnimatedChart()
        {
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

            double animationProgress = 0;
            while (animationProgress < 1)
            {
                ChartCanvas.Children.Clear();

                double startAngle = StartAngle;

                for (int i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    double value = ChartUtilities.GetPropertyValue(item, ValuePath);
                    double sweepAngle = 360 * (value / total);
                    Color sliceColor = GetSliceColor(item, i);
                    DrawSlice(center, radius, startAngle, sweepAngle, sliceColor, item, i, animationProgress, 1 - animationProgress);
                    startAngle += sweepAngle + SliceSpacing;
                }

                if (ShowLegend)
                    DrawLegend(data);

                await Task.Delay(16);

                animationProgress += 0.02;
            }

            DrawChart(1.0, 0);
        }
    }
}
