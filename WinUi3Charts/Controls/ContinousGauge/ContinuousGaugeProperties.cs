using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace WinUi3Charts.Controls
{
    public partial class ContinuousGauge
    {
        public double MinValue
        {
            get => (double)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public double CurrentValue
        {
            get => (double)GetValue(CurrentValueProperty);
            set => SetValue(CurrentValueProperty, value);
        }

        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        public double EndAngle
        {
            get => (double)GetValue(EndAngleProperty);
            set => SetValue(EndAngleProperty, value);
        }

        public Brush ActiveSegmentColor
        {
            get => (Brush)GetValue(ActiveSegmentColorProperty);
            set => SetValue(ActiveSegmentColorProperty, value);
        }

        public Brush DialColor
        {
            get => (Brush)GetValue(DialColorProperty);
            set => SetValue(DialColorProperty, value);
        }

        public bool ShowCurrentValue
        {
            get => (bool)GetValue(ShowCurrentValueProperty);
            set => SetValue(ShowCurrentValueProperty, value);
        }
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Style TextStyle
        {
            get => (Style)GetValue(TextStyleProperty);
            set => SetValue(TextStyleProperty, value);
        }

        public Style CurrentValueStyle
        {
            get => (Style)GetValue(CurrentValueStyleProperty);
            set => SetValue(CurrentValueStyleProperty, value);
        }

        public Brush NonActiveSegmentColor
        {
            get => (Brush)GetValue(NonActiveSegmentColorProperty);
            set => SetValue(NonActiveSegmentColorProperty, value);
        }
    }
}
