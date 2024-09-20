using DPoint = System.Drawing.Point;
using WPoint = System.Windows.Point;

namespace FighterMaker.Visual.Extensions
{
    public static class PointExtensions
    {
        public static DPoint ToSystemDrawingPoint(this WPoint point)
        {
            DPoint p = new DPoint();
            p.X = (int)point.X;
            p.Y = (int)point.Y;

            return p;
        }

        public static WPoint ToSystemWindowsPoint(this DPoint point) 
        {
            WPoint p = new WPoint();
            p.X = point.X;
            p.Y = point.Y;

            return p;
        }
    }
}
