using System.Windows;
using System.Windows.Media.Imaging;

namespace FighterMaker.Visual.Core.Events
{
    public class SpriteSheetEventArgs : EventArgs
    {
        public bool IsEmpty { get => FrameSource is null || FrameRectangle.IsEmpty; }
        public Int32Rect FrameRectangle { get; set; }
        public BitmapSourceSlice? FrameSource { get; set; }

        public SpriteSheetEventArgs(Int32Rect rectangle, BitmapSourceSlice? frame)
        {
            FrameRectangle = rectangle;
            FrameSource = frame;
        }
    }
}
