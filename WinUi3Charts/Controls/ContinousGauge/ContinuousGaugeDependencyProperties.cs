using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI;

namespace WinUi3Charts.Controls
{
    public partial class ContinuousGauge
    {
        public static readonly DependencyProperty MinValueProperty =
             DependencyProperty.Register(nameof(MinValue), typeof(double), typeof(ContinuousGauge), new PropertyMetadata(0.0, OnValueChanged));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(ContinuousGauge), new PropertyMetadata(100.0, OnValueChanged));

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(nameof(CurrentValue), typeof(double), typeof(ContinuousGauge), new PropertyMetadata(0.0, OnValueChanged));

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(ContinuousGauge), new PropertyMetadata(0.0, OnValueChanged));

        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register(nameof(EndAngle), typeof(double), typeof(ContinuousGauge), new PropertyMetadata(360.0, OnValueChanged));

        public static readonly DependencyProperty DialColorProperty =
            DependencyProperty.Register(nameof(DialColor), typeof(Brush), typeof(ContinuousGauge), new PropertyMetadata(new SolidColorBrush(Colors.Transparent), OnValueChanged));

        public static readonly DependencyProperty ActiveSegmentColorProperty =
            DependencyProperty.Register(nameof(ActiveSegmentColor), typeof(Brush), typeof(ContinuousGauge), new PropertyMetadata(new SolidColorBrush(Colors.LightBlue), OnValueChanged));

        public static readonly DependencyProperty ShowCurrentValueProperty =
            DependencyProperty.Register(nameof(ShowCurrentValue), typeof(bool), typeof(ContinuousGauge), new PropertyMetadata(true, OnValueChanged));
        
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(ContinuousGauge), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty TextStyleProperty =
            DependencyProperty.Register(nameof(TextStyle), typeof(Style), typeof(ContinuousGauge), new PropertyMetadata(null, OnAppearanceChanged));

        public static readonly DependencyProperty CurrentValueStyleProperty =
            DependencyProperty.Register(nameof(CurrentValueStyle), typeof(Style), typeof(ContinuousGauge), new PropertyMetadata(null, OnCurrentValueStyleChanged));

        public static readonly DependencyProperty NonActiveSegmentColorProperty =
            DependencyProperty.Register(nameof(NonActiveSegmentColor), typeof(Brush), typeof(ContinuousGauge), new PropertyMetadata(new SolidColorBrush(Colors.Gray), OnValueChanged));
    }
}
