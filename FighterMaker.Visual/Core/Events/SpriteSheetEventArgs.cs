namespace FighterMaker.Visual.Core.Events
{
    public class SpriteSheetEventArgs : EventArgs
    {
        public BitmapSourceSlice? FrameSource { get; set; }

        public SpriteSheetEventArgs(BitmapSourceSlice? frame)
        {
            FrameSource = frame;
        }
    }
}
