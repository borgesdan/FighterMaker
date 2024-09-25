using System.Windows.Media;

namespace FighterMaker.Visual.Core.Extensions
{
    public static class ColorExtensions
    {
        public static System.Drawing.Color ToSystemDrawingColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
