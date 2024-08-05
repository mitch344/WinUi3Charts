using Microsoft.UI.Xaml.Media;
using System.ComponentModel;
using System;
using Windows.Foundation;


namespace WinUi3Charts.Controls
{
    public class Segment : INotifyPropertyChanged
    {
        private Point _startPoint;
        private Point _endPoint;
        private Brush _stroke;
        private Geometry _data;

        public Point StartPoint
        {
            get => _startPoint;
            set { _startPoint = value; OnPropertyChanged(nameof(StartPoint)); }
        }

        public Point EndPoint
        {
            get => _endPoint;
            set { _endPoint = value; OnPropertyChanged(nameof(EndPoint)); }
        }

        public Brush Stroke
        {
            get => _stroke;
            set { _stroke = value; OnPropertyChanged(nameof(Stroke)); }
        }

        public Geometry Data
        {
            get => _data;
            set { _data = value; OnPropertyChanged(nameof(Data)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdatePathData(double startAngle, double fillAngle)
        {
            double centerX = 150;
            double centerY = 150;
            double radiusX = 130;
            double radiusY = 130;

            Point startPoint = GeometryHelper.GetEllipsePoint(centerX, centerY, radiusX, radiusY, startAngle);
            Point endPoint = GeometryHelper.GetEllipsePoint(centerX, centerY, radiusX, radiusY, fillAngle);

            var pathFigure = new PathFigure { StartPoint = startPoint };
            var arcSegment = new ArcSegment
            {
                Point = endPoint,
                Size = new Size(radiusX, radiusY),
                IsLargeArc = Math.Abs(fillAngle - startAngle) >= 180,
                SweepDirection = SweepDirection.Clockwise
            };

            if (Math.Abs(fillAngle - startAngle) < 0.01 || Math.Abs(fillAngle - startAngle) >= 359.99)
            {
                arcSegment.Point = GeometryHelper.GetEllipsePoint(centerX, centerY, radiusX, radiusY, startAngle - 0.01);
                arcSegment.IsLargeArc = true;
            }

            pathFigure.Segments.Add(arcSegment);

            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);
            Data = pathGeometry;
        }
    }
}
