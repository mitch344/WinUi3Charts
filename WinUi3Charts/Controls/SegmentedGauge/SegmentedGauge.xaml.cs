using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using System;
using System.Collections.ObjectModel;
using Microsoft.UI;

namespace WinUi3Charts.Controls
{
    public partial class SegmentedGauge : UserControl
    {
        private ObservableCollection<Segment> segments;

        public SegmentedGauge()
        {
            InitializeComponent();
            segments = new ObservableCollection<Segment>();
            EllipseSegments.ItemsSource = segments;
            UpdateSegments();
        }

        private void CreateEllipseSegments()
        {
            segments.Clear();
            double angleRange = EndAngle - StartAngle;
            double centerX = 150;
            double centerY = 150;
            double radiusX = 130;
            double radiusY = 130;
            double angleIncrement = angleRange / SegmentCount;
            for (int i = 0; i < SegmentCount; i++)
            {
                double startAngle = StartAngle + i * angleIncrement;
                double endAngle = startAngle + angleIncrement;
                Point startPoint = GeometryHelper.GetEllipsePoint(centerX, centerY, radiusX, radiusY, startAngle);
                Point endPoint = GeometryHelper.GetEllipsePoint(centerX, centerY, radiusX, radiusY, endAngle);
                var segment = new Segment
                {
                    StartPoint = startPoint,
                    EndPoint = endPoint,
                    Stroke = NonActiveSegment,
                    Data = CreateArcGeometry(centerX, centerY, radiusX, radiusY, startAngle, endAngle)
                };
                segments.Add(segment);
            }
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

        private void UpdateSegments()
        {
            if (segments == null || segments.Count == 0)
            {
                CreateEllipseSegments();
            }

            if (segments.Count == 0)
            {
                return;
            }

            double angleRange = EndAngle - StartAngle;
            double fillAngle = StartAngle + angleRange * (CurrentValue - MinValue) / (MaxValue - MinValue);

            fillAngle = Math.Min(fillAngle, EndAngle - 0.01);
            double angleIncrement = angleRange / SegmentCount;
            for (int i = 0; i < segments.Count; i++)
            {
                var segment = segments[i];
                double segmentAngle = StartAngle + i * angleIncrement;
                if (segmentAngle < fillAngle && i % 2 == 1)
                {
                    segment.Stroke = ActiveSegmentColor;
                }
                else
                {
                    segment.Stroke = i % 2 == 0 ? new SolidColorBrush(Colors.Transparent) : NonActiveSegment;
                }
            }
        }
    }
}