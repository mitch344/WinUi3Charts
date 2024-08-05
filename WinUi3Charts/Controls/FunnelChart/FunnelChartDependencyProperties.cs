using Microsoft.UI.Xaml;
using Microsoft.UI;
using Windows.UI;
using System.Collections;

namespace WinUi3Charts.Controls
{
    public partial class FunnelChart
    {
        public static readonly DependencyProperty LabelPathProperty =
            DependencyProperty.Register(nameof(LabelPath), typeof(string), typeof(FunnelChart), new PropertyMetadata(string.Empty, OnBindingPathChanged));

        public static readonly DependencyProperty ValuePathProperty =
            DependencyProperty.Register(nameof(ValuePath), typeof(string), typeof(FunnelChart), new PropertyMetadata(string.Empty, OnBindingPathChanged));

        public static readonly DependencyProperty ColorPathProperty =
            DependencyProperty.Register(nameof(ColorPath), typeof(string), typeof(FunnelChart), new PropertyMetadata(string.Empty, OnBindingPathChanged));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(FunnelChart), new PropertyMetadata(null, OnItemsSourceChanged));

        public static readonly DependencyProperty CircleSizeProperty =
            DependencyProperty.Register(nameof(CircleSize), typeof(double), typeof(FunnelChart), new PropertyMetadata(70.0, OnPropertyChanged));

        public static readonly DependencyProperty SectionGapProperty =
            DependencyProperty.Register(nameof(SectionGap), typeof(double), typeof(FunnelChart), new PropertyMetadata(30.0, OnPropertyChanged));

        public static readonly DependencyProperty FunnelStartWidthProperty =
            DependencyProperty.Register(nameof(FunnelStartWidth), typeof(double), typeof(FunnelChart), new PropertyMetadata(double.NaN, OnPropertyChanged));

        public static readonly DependencyProperty FunnelEndWidthProperty =
            DependencyProperty.Register(nameof(FunnelEndWidth), typeof(double), typeof(FunnelChart), new PropertyMetadata(double.NaN, OnPropertyChanged));

        public static readonly DependencyProperty ConnectingLineThicknessProperty =
            DependencyProperty.Register(nameof(ConnectingLineThickness), typeof(double), typeof(FunnelChart), new PropertyMetadata(2.0, OnPropertyChanged));

        public static readonly DependencyProperty ConnectingLineColorProperty =
            DependencyProperty.Register(nameof(ConnectingLineColor), typeof(Color), typeof(FunnelChart), new PropertyMetadata(Colors.LightGray, OnPropertyChanged));

        public static readonly DependencyProperty LabelStyleProperty =
            DependencyProperty.Register(nameof(LabelStyle), typeof(Style), typeof(FunnelChart), new PropertyMetadata(null, OnPropertyChanged));

        public static readonly DependencyProperty ValueStyleProperty =
            DependencyProperty.Register(nameof(ValueStyle), typeof(Style), typeof(FunnelChart), new PropertyMetadata(null, OnPropertyChanged));

        public static readonly DependencyProperty CircleStyleProperty =
            DependencyProperty.Register(nameof(CircleStyle), typeof(Style), typeof(FunnelChart), new PropertyMetadata(null, OnPropertyChanged));

        public static readonly DependencyProperty Use3DEffectProperty =
            DependencyProperty.Register(nameof(Use3DEffect), typeof(bool), typeof(FunnelChart), new PropertyMetadata(false, OnPropertyChanged));

        public static readonly DependencyProperty Depth3DProperty =
            DependencyProperty.Register(nameof(Depth3D), typeof(double), typeof(FunnelChart), new PropertyMetadata(10.0, OnPropertyChanged));

        public static readonly DependencyProperty UseSphereForCirclesProperty =
            DependencyProperty.Register(nameof(UseSphereForCircles), typeof(bool), typeof(FunnelChart), new PropertyMetadata(false, OnPropertyChanged));

        public static readonly DependencyProperty ShowCirclesProperty =
            DependencyProperty.Register(nameof(ShowCircles), typeof(bool), typeof(FunnelChart), new PropertyMetadata(true, OnPropertyChanged));
    }
}
