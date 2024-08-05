using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections;
namespace WinUi3Charts.Controls
{
    public sealed partial class PieChart : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(PieChart), new PropertyMetadata(null, OnItemsSourceChanged));

        public static readonly DependencyProperty ValuePathProperty =
            DependencyProperty.Register("ValuePath", typeof(string), typeof(PieChart), new PropertyMetadata(string.Empty, OnPathChanged));

        public static readonly DependencyProperty LabelPathProperty =
            DependencyProperty.Register("LabelPath", typeof(string), typeof(PieChart), new PropertyMetadata(string.Empty, OnPathChanged));

        public static readonly DependencyProperty SliceColorPathProperty =
            DependencyProperty.Register("SliceColorPath", typeof(string), typeof(PieChart), new PropertyMetadata(string.Empty, OnPathChanged));

        public static readonly DependencyProperty SliceStyleProperty =
            DependencyProperty.Register("SliceStyle", typeof(SliceStyle), typeof(PieChart), new PropertyMetadata(SliceStyle.Flat, OnAppearanceChanged));

        public static readonly DependencyProperty ShowLabelsProperty =
            DependencyProperty.Register("ShowLabels", typeof(bool), typeof(PieChart), new PropertyMetadata(true, OnAppearanceChanged));

        public static readonly DependencyProperty ShowValuesProperty =
            DependencyProperty.Register("ShowValues", typeof(bool), typeof(PieChart), new PropertyMetadata(true, OnAppearanceChanged));

        public static readonly DependencyProperty ShowLegendProperty =
            DependencyProperty.Register("ShowLegend", typeof(bool), typeof(PieChart), new PropertyMetadata(false, OnAppearanceChanged));

        public static readonly DependencyProperty LegendPositionProperty =
            DependencyProperty.Register("LegendPosition", typeof(LegendPosition), typeof(PieChart), new PropertyMetadata(LegendPosition.Right, OnAppearanceChanged));

        public static readonly DependencyProperty SliceSpacingProperty =
            DependencyProperty.Register("SliceSpacing", typeof(double), typeof(PieChart), new PropertyMetadata(0.0, OnAppearanceChanged));

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(PieChart), new PropertyMetadata(0.0, OnAppearanceChanged));

        public static readonly DependencyProperty SelectedSliceIndexProperty =
            DependencyProperty.Register("SelectedSliceIndex", typeof(int), typeof(PieChart), new PropertyMetadata(-1, OnSelectionChanged));

        public static readonly DependencyProperty LabelStyleProperty =
            DependencyProperty.Register(nameof(LabelStyle), typeof(Style), typeof(PieChart), new PropertyMetadata(null, OnAppearanceChanged));

        public static readonly DependencyProperty LegendItemStyleProperty =
            DependencyProperty.Register(nameof(LegendItemStyle), typeof(Style), typeof(PieChart), new PropertyMetadata(null, OnAppearanceChanged));

        public static readonly DependencyProperty AnimateOnLoadProperty =
            DependencyProperty.Register(nameof(AnimateOnLoad), typeof(bool), typeof(PieChart), new PropertyMetadata(null, OnAppearanceChanged));
    }
}
