using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Globalization;
using System.Windows.Media;

namespace Project_Stereometry
{
    public class BackgroundToGradientColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("The target must be a brush!");

            SolidColorBrush background = value as SolidColorBrush;
            Color lightColor = background.Color;
            float r = lightColor.ScR;
            float g = lightColor.ScG;
            float b = lightColor.ScB;

            Color darkColor = new Color();
            darkColor.ScA = 1;
            darkColor.ScR = (float)(r * 0.28672 * 2.1);
            darkColor.ScG = (float)(g * 0.14117 * 1.9);
            darkColor.ScB = (float)(b * 0.30196 * 1.6);

            Brush gradient = new LinearGradientBrush(
                lightColor,
                darkColor,
                60.0
                );

            return gradient;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
