using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace WinUi3Charts.Controls
{
    public sealed partial class LineChart : UserControl
    {
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as LineChart;
            chart?.UpdateChart();
        }

        private static void OnPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as LineChart;
            chart?.UpdateChart();
        }

        private static void OnAppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as LineChart;
            chart?.UpdateChart();
        }

        private void LineChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateChart();
        }

        private static void OnSeriesCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as LineChart;
            if (chart != null)
            {
                if (e.OldValue is ObservableCollection<LineSeries> oldCollection)
                {
                    oldCollection.CollectionChanged -= chart.SeriesCollection_CollectionChanged;
                }

                if (e.NewValue is ObservableCollection<LineSeries> newCollection)
                {
                    newCollection.CollectionChanged += chart.SeriesCollection_CollectionChanged;
                }

                chart.UpdateChart();
            }
        }

        private void SeriesCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateChart();
        }

        private void LineChart_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateChart(true);
        }
    }
}
