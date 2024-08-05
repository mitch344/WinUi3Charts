using Windows.Foundation;
using System;

namespace WinUi3Charts.Controls
{
    public static class GeometryHelper
    {
        public static Point GetEllipsePoint(double centerX, double centerY, double radiusX, double radiusY, double angle)
        {
            double radians = angle * Math.PI / 180;
            double x = centerX + radiusX * Math.Cos(radians);
            double y = centerY + radiusY * Math.Sin(radians);
            return new Point(x, y);
        }
    }
}
