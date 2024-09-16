using FighterMaker.Mono.Library.Enumerations;
using Microsoft.Xna.Framework;

namespace FighterMaker.Mono.Library.Extensions
{
    public static class OriginPointExtensions
    {
        public static Vector2 ToVector2(this OriginPoint originPoint, Rectangle bounds)
        {
            switch (originPoint)
            {
                case OriginPoint.TopLeft:
                    return Vector2.Zero;
                case OriginPoint.TopCenter:
                    return new Vector2(bounds.Center.X, 0);
                case OriginPoint.TopRight:
                    return new Vector2(bounds.Right, 0);
                case OriginPoint.Right:
                    return new Vector2(bounds.Right, bounds.Center.Y);
                case OriginPoint.BottomRight:
                    return new Vector2(bounds.Right, bounds.Bottom);
                case OriginPoint.BottomCenter:
                    return new Vector2(bounds.Center.X, bounds.Bottom);
                case OriginPoint.BottomLeft:
                    return new Vector2(bounds.Left, bounds.Bottom);
                case OriginPoint.Left:
                    return new Vector2(bounds.Left, bounds.Center.Y);
                case OriginPoint.Center:
                    var center = bounds.Center;
                    return new Vector2(center.X, center.Y);
                default:
                    return Vector2.Zero;
            }
        }
    }
}
