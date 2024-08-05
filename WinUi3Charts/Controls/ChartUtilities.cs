using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using System.Collections;
using System.Linq;
using System;

namespace WinUi3Charts.Controls
{
    public static class ChartUtilities
    {
        public static void ShowErrorMessage(string message, Canvas ChartCanvas)
        {
            var errorMessage = new TextBlock
            {
                Text = message,
                Foreground = new SolidColorBrush(Colors.Red),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            Canvas.SetLeft(errorMessage, (ChartCanvas.ActualWidth - errorMessage.ActualWidth) / 2);
            Canvas.SetTop(errorMessage, (ChartCanvas.ActualHeight - errorMessage.ActualHeight) / 2);

            ChartCanvas.Children.Add(errorMessage);
        }

        public static bool ValidateData(IEnumerable ItemsSource, string ValuePath, string LabelPath)
        {
            return ItemsSource != null && !string.IsNullOrEmpty(ValuePath) && !string.IsNullOrEmpty(LabelPath) && ItemsSource.Cast<object>().Any();
        }

        public static object GetPropertyObjectValue(object item, string propertyName)
        {
            if (item == null || string.IsNullOrEmpty(propertyName))
                return null;

            var property = item.GetType().GetProperty(propertyName);
            return property?.GetValue(item);
        }

        public static string GetPropertyStringValue(object item, string propertyName)
        {
            if (item == null || string.IsNullOrEmpty(propertyName))
                return string.Empty;

            var property = item.GetType().GetProperty(propertyName);
            return property?.GetValue(item)?.ToString() ?? string.Empty;
        }

        public static double GetPropertyValue(object item, string propertyName)
        {
            if (item == null || string.IsNullOrEmpty(propertyName))
                return 0;

            var property = item.GetType().GetProperty(propertyName);
            if (property != null)
            {
                var value = property.GetValue(item);
                if (value is double doubleValue)
                {
                    return doubleValue;
                }
                else if (value is int intValue)
                {
                    return intValue;
                }
                else if (value is float floatValue)
                {
                    return floatValue;
                }
                else if (value is long longValue)
                {
                    return longValue;
                }
            }
            return 0;
        }
    }
}
