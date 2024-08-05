using Microsoft.UI.Xaml;

namespace WinUi3Charts.Controls
{
    public sealed partial class BarChart
    {
        private static void OnAppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as BarChart;
            chart?.UpdateChart();
        }

        private static void OnFontChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as BarChart;
            chart?.UpdateChart();
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as BarChart;
            chart?.UpdateChart();
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as BarChart;
            chart?.UpdateChart();
        }

        private static void OnPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as BarChart;
            chart?.UpdateChart();
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as BarChart;
            chart?.UpdateChart();
        }

        private static void OnBarColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as BarChart;
            chart?.UpdateChart();
        }
        private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = (BarChart)d;
            chart.UpdateChart();
        }

        private void BarChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateChart();
        }
    }
}
