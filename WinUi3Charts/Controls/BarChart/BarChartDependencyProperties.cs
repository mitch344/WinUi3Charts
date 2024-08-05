using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using System.Collections;
using Windows.UI;

namespace WinUi3Charts.Controls
{
    public sealed partial class BarChart
    {
        public static readonly DependencyProperty BarStyleProperty =
            DependencyProperty.Register("BarStyle", typeof(BarStyle), typeof(BarChart), new PropertyMetadata(BarStyle.Flat, OnAppearanceChanged));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(BarChart), new PropertyMetadata(null, OnItemsSourceChanged));

        public static readonly DependencyProperty ValuePathProperty =
            DependencyProperty.Register("ValuePath", typeof(string), typeof(BarChart), new PropertyMetadata(string.Empty, OnPathChanged));

        public static readonly DependencyProperty LabelPathProperty =
            DependencyProperty.Register("LabelPath", typeof(string), typeof(BarChart), new PropertyMetadata(string.Empty, OnPathChanged));

        public static readonly DependencyProperty IndividualBarColorPathProperty =
            DependencyProperty.Register("IndividualBarColorPath", typeof(string), typeof(BarChart), new PropertyMetadata(string.Empty, OnPathChanged));

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(BarChart), new PropertyMetadata(Orientation.Vertical, OnOrientationChanged));

        public static readonly DependencyProperty BarColorProperty =
            DependencyProperty.Register("BarColor", typeof(Color), typeof(BarChart), new PropertyMetadata(Colors.Blue, OnBarColorChanged));

        public static readonly DependencyProperty LabelStyleProperty =
            DependencyProperty.Register(nameof(LabelStyle), typeof(Style), typeof(BarChart), new PropertyMetadata(null, OnStyleChanged));

        public static readonly DependencyProperty ValueLabelStyleProperty =
            DependencyProperty.Register(nameof(ValueLabelStyle), typeof(Style), typeof(BarChart), new PropertyMetadata(null, OnStyleChanged));
    }
}
