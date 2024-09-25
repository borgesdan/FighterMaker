using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FighterMaker.Visual.Core.Extensions
{
    public static class ImageControlExtensions
    {
        public static void ResetCanvasTopLeftProperties(this Image image)
        {
            image.SetValue(Canvas.LeftProperty, 0.0);
            image.SetValue(Canvas.TopProperty, 0.0);
        }

        public static Tuple<double, double> GetCanvasLeftTopProperties(this Image image)
        {
            var left = (double)image.GetValue(Canvas.LeftProperty);
            var top = (double)image.GetValue(Canvas.TopProperty);

            return new Tuple<double, double>(left, top);
        }
    }
}
