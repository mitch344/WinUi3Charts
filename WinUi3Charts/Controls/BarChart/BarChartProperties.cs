using Microsoft.UI.Xaml.Controls;
using System.Collections;
using Windows.UI;
using Microsoft.UI.Xaml;
using System.Linq;

namespace WinUi3Charts.Controls
{
    public sealed partial class BarChart
    {
        public BarStyle BarStyle
        {
            get { return (BarStyle)GetValue(BarStyleProperty); }
            set { SetValue(BarStyleProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty) ?? Enumerable.Empty<object>(); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public string ValuePath
        {
            get { return (string)GetValue(ValuePathProperty) ?? string.Empty; }
            set { SetValue(ValuePathProperty, value); }
        }

        public string LabelPath
        {
            get { return (string)GetValue(LabelPathProperty) ?? string.Empty; }
            set { SetValue(LabelPathProperty, value); }
        }

        public string IndividualBarColorPath
        {
            get { return (string)GetValue(IndividualBarColorPathProperty) ?? string.Empty; }
            set { SetValue(IndividualBarColorPathProperty, value); }
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public Color BarColor
        {
            get { return (Color)GetValue(BarColorProperty); }
            set { SetValue(BarColorProperty, value); }
        }

        public Style LabelStyle
        {
            get => (Style)GetValue(LabelStyleProperty);
            set => SetValue(LabelStyleProperty, value);
        }

        public Style ValueLabelStyle
        {
            get => (Style)GetValue(ValueLabelStyleProperty);
            set => SetValue(ValueLabelStyleProperty, value);
        }
    }
}
