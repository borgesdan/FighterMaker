using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FighterMaker.Visual.Core.Helpers
{
    public static class BitmapHelper
    {
        public static BitmapSource BitmapSourceFromArray(byte[] data, int w, int h, int ch, double dpiX = 96, double dpiY = 96)
        {
            PixelFormat format = PixelFormats.Default;

            if (ch == 1) format = PixelFormats.Gray8; //grey scale image 0-255
            if (ch == 3) format = PixelFormats.Bgr24; //RGB
            if (ch == 4) format = PixelFormats.Bgr32; //RGB + alpha

            WriteableBitmap wbm = new WriteableBitmap(w, h, dpiX, dpiY, format, null);
            wbm.WritePixels(new Int32Rect(0, 0, w, h), data, ch * w, 0);

            return wbm;
        }
    }
}
