using Microsoft.UI.Xaml;
using System.Collections;
using System.Linq;
using Windows.UI;

namespace WinUi3Charts.Controls
{
    public partial class FunnelChart
    {
        public double CircleSize
        {
            get => (double)GetValue(CircleSizeProperty);
            set => SetValue(CircleSizeProperty, value);
        }

        public double SectionGap
        {
            get => (double)GetValue(SectionGapProperty);
            set => SetValue(SectionGapProperty, value);
        }

        public double FunnelStartWidth
        {
            get => (double)GetValue(FunnelStartWidthProperty);
            set => SetValue(FunnelStartWidthProperty, value);
        }

        public double FunnelEndWidth
        {
            get => (double)GetValue(FunnelEndWidthProperty);
            set => SetValue(FunnelEndWidthProperty, value);
        }

        public double ConnectingLineThickness
        {
            get => (double)GetValue(ConnectingLineThicknessProperty);
            set => SetValue(ConnectingLineThicknessProperty, value);
        }

        public Color ConnectingLineColor
        {
            get => (Color)GetValue(ConnectingLineColorProperty);
            set => SetValue(ConnectingLineColorProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty) ?? Enumerable.Empty<object>();
            set => SetValue(ItemsSourceProperty, value);
        }

        public string LabelPath
        {
            get => (string)GetValue(LabelPathProperty) ?? string.Empty;
            set => SetValue(LabelPathProperty, value);
        }

        public string ValuePath
        {
            get => (string)GetValue(ValuePathProperty) ?? string.Empty;
            set => SetValue(ValuePathProperty, value);
        }

        public string ColorPath
        {
            get => (string)GetValue(ColorPathProperty) ?? string.Empty;
            set => SetValue(ColorPathProperty, value);
        }

        public Style LabelStyle
        {
            get => (Style)GetValue(LabelStyleProperty);
            set => SetValue(LabelStyleProperty, value);
        }

        public Style ValueStyle
        {
            get => (Style)GetValue(ValueStyleProperty);
            set => SetValue(ValueStyleProperty, value);
        }

        public Style CircleStyle
        {
            get => (Style)GetValue(CircleStyleProperty);
            set => SetValue(CircleStyleProperty, value);
        }

        public bool Use3DEffect
        {
            get => (bool)GetValue(Use3DEffectProperty);
            set => SetValue(Use3DEffectProperty, value);
        }

        public double Depth3D
        {
            get => (double)GetValue(Depth3DProperty);
            set => SetValue(Depth3DProperty, value);
        }

        public bool UseSphereForCircles
        {
            get => (bool)GetValue(UseSphereForCirclesProperty);
            set => SetValue(UseSphereForCirclesProperty, value);
        }

        public bool ShowCircles
        {
            get => (bool)GetValue(ShowCirclesProperty);
            set => SetValue(ShowCirclesProperty, value);
        }
    }
}
