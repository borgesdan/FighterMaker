using System.Windows;

namespace FighterMaker.Visual.Core.Extensions
{
    public static class RectangleExtensions
    {
        public static Rect ToSystemWindowsInt32Rect(this System.Drawing.Rectangle rectangle)
        {
            Rect rect = new Rect();
            rect.X = rectangle.X;
            rect.Y = rectangle.Y;
            rect.Width = rectangle.Width;
            rect.Height = rectangle.Height;

            return rect;
        }

        public static System.Drawing.Rectangle ToSystemDrawinRectangle(this Int32Rect rect)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle();
            rectangle.X = rect.X;
            rectangle.Y = rect.Y;
            rectangle.Width = rect.Width;
            rectangle.Height = rect.Height;

            return rectangle;
        }
    }
}
