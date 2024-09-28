using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace FighterMaker.Visual.Core
{
    public class BitmapSourceSlice
    {
        readonly BitmapSource bitmap;
        Int32Rect sourceRectangle;
        CroppedBitmap? cropped;

        /// <summary>
        /// Gets the source bitmap.
        /// </summary>
        public BitmapSource Source => bitmap;

        /// <summary>
        /// Gets the aspect ratio of (bits per pixel / 8) * the width of the source rectangle.
        /// </summary>
        public int Stride => sourceRectangle.Width * (bitmap.Format.BitsPerPixel / 8);

        public CroppedBitmap Cropped
        {
            get
            {
                cropped ??= new CroppedBitmap(bitmap, sourceRectangle);

                return cropped;
            }
        }

        /// <summary>
        /// Obtém o retângulo de origem.
        /// </summary>
        public Int32Rect SourceRectangle
        {
            get => sourceRectangle;
            set
            {
                sourceRectangle = value;

                if (sourceRectangle.Width > bitmap.Width)
                    sourceRectangle.Width = (int)bitmap.Width;
                if (sourceRectangle.Height > bitmap.Height)
                    sourceRectangle.Height = (int)bitmap.Height;
            }
        }

        public BitmapSourceSlice(BitmapSource source, Int32Rect sourceRectangle)
        {
            this.bitmap = source ?? throw new ArgumentNullException(nameof(source));
            SourceRectangle = sourceRectangle;            
        }                

        /// <summary>
        /// Obtém um retângulo ajustado, a partir do retângulo de origem, com a exclusão da área de fundo da imagem.
        /// </summary>
        public Int32Rect GetFittedRectangle()
        {            
            var perPixelRatio = bitmap.Format.BitsPerPixel / 8;

            switch (perPixelRatio)
            {
                case 1: //grey scale image 0-255
                    byte[] byteArray = new byte[sourceRectangle.Width * sourceRectangle.Height];
                    bitmap.CopyPixels(sourceRectangle, byteArray, Stride, 0);
                    return GetFittedRectanglePerFormat(byteArray);
                case 3: //RGB
                    short[] shortArray = new short[sourceRectangle.Width * sourceRectangle.Height];
                    bitmap.CopyPixels(sourceRectangle, shortArray, Stride, 0);
                    return GetFittedRectanglePerFormat(shortArray);
                case 4: //RGB + alpha
                    int[] intArray = new int[sourceRectangle.Width * sourceRectangle.Height];
                    bitmap.CopyPixels(sourceRectangle, intArray, Stride, 0);
                    return GetFittedRectanglePerFormat(intArray);
                default:
                    throw new NotImplementedException();
            }            
        }

        private Int32Rect GetFittedRectanglePerFormat<T>(T[] pixelArray) where T : struct
        {
            var y = 0;
            var x = 0;
            var width = 0;
            var height = 0;

            //Definimos o array de pixels em linhas e colunas
            for (int col = 0; col < sourceRectangle.Width; ++col)
            {
                for (int row = 0; row < sourceRectangle.Height; ++row)
                {
                    //Verificamos os pixels adjacentes e fazemos comparações

                    var currentPixel = pixelArray[col + (row * sourceRectangle.Width)];

                    if (col + 1 < sourceRectangle.Width)
                    {
                        var rightPixel = pixelArray[col + 1 + (row * sourceRectangle.Width)];

                        if (!EqualityComparer<T>.Default.Equals(currentPixel, rightPixel))
                        {
                            if (x == 0 || col + 1 < x)  //Encontramos o limite esquerdo
                                x = col + 1;
                            else if (col + 1 > width) //Encontramos o limite direito
                                width = col + 1;
                        }
                    }

                    if (row + 1 < sourceRectangle.Height)
                    {
                        var bottomPixel = pixelArray[col + ((row + 1) * sourceRectangle.Width)];

                        if (!EqualityComparer<T>.Default.Equals(currentPixel, bottomPixel))
                        {
                            if (y == 0 || row + 1 < y) //Encontramos o limite do topo
                                y = row + 1;
                            else if (row + 1 > height)  //Encontramos o limite da base
                                height = row + 1;
                        }
                    }
                }
            }

            var rect = new Int32Rect(x, y, width, height);
            return rect;
        }
    }
}

//DOCS
//https://stackoverflow.com/questions/8311805/bitmapsource-copypixels-byte-bitmapsource-how-to-do-this-simple
//https://medium.com/@oleg.shipitko/what-does-stride-mean-in-image-processing-bba158a72bcd
//https://stackoverflow.com/questions/16220472/how-to-create-a-bitmapimage-from-a-pixel-byte-array-live-video-display
//https://stackoverflow.com/questions/14876989/how-to-read-pixels-in-four-corners-of-a-bitmapsource
