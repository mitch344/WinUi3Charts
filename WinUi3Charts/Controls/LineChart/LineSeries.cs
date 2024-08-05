using System.Collections;
using Windows.UI;

namespace WinUi3Charts.Controls
{
    public class LineSeries
    {
        public string Name { get; set; }
        public IEnumerable ItemsSource { get; set; }
        public string XValuePath { get; set; }
        public string YValuePath { get; set; }
        public Color LineColor { get; set; }
        public double LineThickness { get; set; }
        public Color? FillColor { get; set; }
        public FillStyle FillStyle { get; set; }
        public bool ShowDataPoints { get; set; }
        public double DataPointSize { get; set; }
        public Color DataPointColor { get; set; }
    }
}
