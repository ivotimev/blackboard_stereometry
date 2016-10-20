using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project_Stereometry
{
    public class BB_Expander : Expander
    {
        public double ElipseRadius
        {
            get {
                LengthConverter myLenghtConverter = new LengthConverter();
                return (double)myLenghtConverter.ConvertFromString(GetValue(ElipseRadiusProperty) as string);
            }
            set { SetValue(ElipseRadiusProperty, value); }
        }
        public static DependencyProperty ElipseRadiusProperty = DependencyProperty.Register("ElipseRadius", typeof(double), typeof(BB_Expander));

        public string BB_TooltipTitle
        {
            get
            {
                return GetValue(BB_TooltipTitleProperty) as string;
            }
            set { SetValue(BB_TooltipTitleProperty, value); }
        }
        public static DependencyProperty BB_TooltipTitleProperty = DependencyProperty.Register("BB_TooltipTitle", typeof(string), typeof(BB_Expander));

        public string BB_TooltipContent
        {
            get
            {
                return GetValue(BB_TooltipContentProperty) as string;
            }
            set { SetValue(BB_TooltipContentProperty, value); }
        }
        public static DependencyProperty BB_TooltipContentProperty = DependencyProperty.Register("BB_TooltipContent", typeof(string), typeof(BB_Expander));

        public ImageSource TooltipIconSource
        {
            get { return base.GetValue(TooltipIconSourceProperty) as ImageSource; }
            set { base.SetValue(TooltipIconSourceProperty, value); }
        }
        public static readonly DependencyProperty TooltipIconSourceProperty =
          DependencyProperty.Register("TooltipIconSource", typeof(ImageSource), typeof(BB_Expander));
    }
}
