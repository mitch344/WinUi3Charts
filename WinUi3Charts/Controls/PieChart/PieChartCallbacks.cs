using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;


namespace WinUi3Charts.Controls
{
    public sealed partial class PieChart : UserControl
    {
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as PieChart;
            chart?.UpdateChart();
        }

        private static void OnPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as PieChart;
            chart?.UpdateChart();
        }

        private static void OnAppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as PieChart;
            chart?.UpdateChart();
        }

        private static void OnSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as PieChart;
            chart?.UpdateSelection();
        }

        private void Slice_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is Path slice)
            {
                SelectedSliceIndex = (int)slice.Tag;
            }
        }
        private void PieChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateChart();
        }

        private void PieChart_Loaded(object sender, RoutedEventArgs e)
        {

            if (_isInitialLoad)
            {
                _isInitialLoad = false;
                UpdateChart(true);
            }
        }
    }
}
