using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections;
using System.Linq;

namespace WinUi3Charts.Controls
{
    public sealed partial class PieChart : UserControl
    {
        private bool _isInitialLoad = true;

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty) ?? Enumerable.Empty<object>();
            set => SetValue(ItemsSourceProperty, value);
        }

        public string ValuePath
        {
            get => (string)GetValue(ValuePathProperty) ?? string.Empty;
            set => SetValue(ValuePathProperty, value);
        }

        public string LabelPath
        {
            get => (string)GetValue(LabelPathProperty) ?? string.Empty;
            set => SetValue(LabelPathProperty, value);
        }

        public SliceStyle SliceStyle
        {
            get => (SliceStyle)GetValue(SliceStyleProperty);
            set => SetValue(SliceStyleProperty, value);
        }

        public bool ShowLabels
        {
            get => (bool)GetValue(ShowLabelsProperty);
            set => SetValue(ShowLabelsProperty, value);
        }

        public bool ShowValues
        {
            get => (bool)GetValue(ShowValuesProperty);
            set => SetValue(ShowValuesProperty, value);
        }

        public bool ShowLegend
        {
            get => (bool)GetValue(ShowLegendProperty);
            set => SetValue(ShowLegendProperty, value);
        }

        public LegendPosition LegendPosition
        {
            get => (LegendPosition)GetValue(LegendPositionProperty);
            set => SetValue(LegendPositionProperty, value);
        }

        public double SliceSpacing
        {
            get => (double)GetValue(SliceSpacingProperty);
            set => SetValue(SliceSpacingProperty, value);
        }

        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        public int SelectedSliceIndex
        {
            get => (int)GetValue(SelectedSliceIndexProperty);
            set => SetValue(SelectedSliceIndexProperty, value);
        }

        public string SliceColorPath
        {
            get => (string)GetValue(SliceColorPathProperty) ?? string.Empty;
            set => SetValue(SliceColorPathProperty, value);
        }

        public Style LabelStyle
        {
            get => (Style)GetValue(LabelStyleProperty);
            set => SetValue(LabelStyleProperty, value);
        }

        public Style LegendItemStyle
        {
            get => (Style)GetValue(LegendItemStyleProperty);
            set => SetValue(LegendItemStyleProperty, value);
        }

        public bool AnimateOnLoad
        {
            get => (bool)GetValue(AnimateOnLoadProperty);
            set => SetValue(AnimateOnLoadProperty, value);
        }
    }
}
