using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using System;

namespace WinUi3Charts.Controls
{
    public partial class ContinuousGauge : UserControl
    {
        public ContinuousGauge()
        {
            InitializeComponent();
            Loaded += ContinuousGauge_Loaded;
            UpdateGauge();
        }
        private void UpdateGauge()
        {
            double centerX = 150;
            double centerY = 150;
            double radiusX = 130;
            double radiusY = 130;

            nonActiveSegmentPath.Data = CreateArcGeometry(centerX, centerY, radiusX, radiusY, StartAngle, EndAngle);
            nonActiveSegmentPath.Stroke = NonActiveSegmentColor;

            double angleRange = EndAngle - StartAngle;
            double fillAngle = StartAngle + angleRange * (CurrentValue - MinValue) / (MaxValue - MinValue);
            fillAngle = Math.Min(fillAngle, EndAngle - 0.01);

            foregroundPath.Data = CreateArcGeometry(centerX, centerY, radiusX, radiusY, StartAngle, fillAngle);
            foregroundPath.Stroke = ActiveSegmentColor;
        }

        private Geometry CreateArcGeometry(double centerX, double centerY, double radiusX, double radiusY, double startAngle, double endAngle)
        {
            Point startPoint = GeometryHelper.GetEllipsePoint(centerX, centerY, radiusX, radiusY, startAngle);
            Point endPoint = GeometryHelper.GetEllipsePoint(centerX, centerY, radiusX, radiusY, endAngle);

            var pathFigure = new PathFigure { StartPoint = startPoint };
            var arcSegment = new ArcSegment
            {
                Point = endPoint,
                Size = new Size(radiusX, radiusY),
                IsLargeArc = Math.Abs(endAngle - startAngle) > 180,
                SweepDirection = SweepDirection.Clockwise
            };
            pathFigure.Segments.Add(arcSegment);

            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);
            return pathGeometry;
        }
    }
}