using Microsoft.UI.Xaml;

namespace WinUi3Charts.Controls
{
    public partial class FunnelChart
    {
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = (FunnelChart)d;
            chart.UpdateLayout();
            chart.DrawFunnel();
        }

        private void FunnelChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawFunnel();
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as FunnelChart;
            chart?.DrawFunnel();
        }

        private static void OnBindingPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as FunnelChart;
            chart?.DrawFunnel();
        }
    }
}

