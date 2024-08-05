using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUi3Charts.Controls
{
    public partial class ContinuousGauge
    {
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ContinuousGauge gauge)
            {
                gauge.UpdateGauge();
            }
        }

        private static void OnAppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ContinuousGauge gauge)
            {
                gauge.UpdateStyles();
            }
        }

        private static void OnCurrentValueStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ContinuousGauge gauge)
            {
                gauge.UpdateCurrentValueStyle();
            }
        }

        private void UpdateCurrentValueStyle()
        {
            var currentValueTextBlock = FindName("CurrentValueTextBlock") as TextBlock;
            if (currentValueTextBlock != null)
            {
                currentValueTextBlock.Style = CurrentValueStyle ??
                    (Style)Resources["DefaultCurrentValueStyle"];
            }
        }

        private void UpdateTextStyle()
        {
            var textBlock = FindName("TextBlock") as TextBlock;
            if (textBlock != null)
            {
                textBlock.Style = TextStyle ??
                    (Style)Resources["DefaultTextStyle"];
            }
        }

        private void UpdateStyles()
        {
            UpdateCurrentValueStyle();
            UpdateTextStyle();
        }

        private void ContinuousGauge_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCurrentValueStyle();
            UpdateStyles();
            UpdateGauge();
        }
    }
}
