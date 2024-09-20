using System.Drawing;
using System.IO;

//https://simpledevcode.wordpress.com/2015/12/29/flood-fill-algorithm-using-c-net/

namespace FighterMaker.Visual.Core
{
    public class FloodFillAlgorithm
    {
        Bitmap bmp;
        Rectangle frame;

        public int MaxY { get; private set; }
        public int MaxX { get; private set; }
        public int MinX { get; private set; }
        public int MinY { get; private set; }

        //Altura, Largura
        //Linha, Colunas
        bool[,] map = null;

        public FloodFillAlgorithm(Stream stream, Rectangle? frameRectangle = null)
        {
            bmp = new Bitmap(stream);

            if (frameRectangle == null)
            {
                frame.X = 0;
                frame.Y = 0;
                frame.Width = bmp.Width;
                frame.Height = bmp.Height;
            }
            else
            {
                this.frame = frameRectangle.Value;
            }

            MinX = frame.Width;
            MinY = frame.Height;
            //map = new bool[this.frame.Height, this.frame.Width];
        }

        public void FloodFill(Point pt, Color targetColor, Color replacementColor)
        {
            Stack<Point> pixels = new Stack<Point>();
            targetColor = bmp.GetPixel(pt.X, pt.Y);
            pixels.Push(pt);

            while (pixels.Count > 0)
            {
                Point a = pixels.Pop();
                if (a.X < frame.Right && a.X > 0 && a.Y < frame.Bottom && a.Y > 0)//make sure we stay within bounds
                {
                    if (bmp.GetPixel(a.X, a.Y) == targetColor)
                    {
                        bmp.SetPixel(a.X, a.Y, replacementColor);

                        pixels.Push(new Point(a.X - 1, a.Y));
                        pixels.Push(new Point(a.X + 1, a.Y));
                        pixels.Push(new Point(a.X, a.Y - 1));
                        pixels.Push(new Point(a.X, a.Y + 1));
                    }
                }
            }
        }
    }
}
