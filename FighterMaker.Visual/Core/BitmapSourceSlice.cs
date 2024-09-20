using System.Windows;
using System.Windows.Media.Imaging;

namespace FighterMaker.Visual.Core
{
    public class BitmapSourceSlice
    {
        BitmapSource bitmap;
        int[] pixels;
        Int32Rect sourceRectangle;

        /// <summary>
        /// Obtém o bitmap de origem.
        /// </summary>
        public BitmapSource Source => bitmap;

        /// <summary>
        /// Obtém a proporção de (bits por pixel / 8) * a largura do retângulo de origem.
        /// </summary>
        public int Stride => sourceRectangle.Width * (bitmap.Format.BitsPerPixel / 8);

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

            pixels = new int[sourceRectangle.Width * sourceRectangle.Height];
        }

        void CopyPixels()
        {
            bitmap.CopyPixels(sourceRectangle, pixels, Stride, 0);
        }

        /// <summary>
        /// Obtém um retângulo ajustado, a partir do retângulo de origem, com a exclusão da área de fundo da imagem.
        /// </summary>
        public Int32Rect GetFittedRectangle()
        {
            CopyPixels();

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

                    var currentPixel = pixels[col + (row * sourceRectangle.Width)];

                    if (col + 1 < sourceRectangle.Width)
                    {
                        var rightPixel = pixels[col + 1 + (row * sourceRectangle.Width)];

                        if (currentPixel != rightPixel)
                        {
                            if (x == 0 || col + 1 < x)  //Encontramos o limite esquerdo
                                x = col + 1;
                            else if (col + 1 > width) //Encontramos o limite direito
                                width = col + 1;
                        }
                    }

                    if (row + 1 < sourceRectangle.Height)
                    {
                        var bottomPixel = pixels[col + ((row + 1) * sourceRectangle.Width)];

                        if (currentPixel != bottomPixel)
                        {
                            if (y == 0 || row + 1 < y) //Encontramos o limite do topo
                                y = row + 1;
                            else if (row + 1 > height)  //Encontramos o limite da base
                                height = row + 1;
                        }
                    }
                }
            }

            Int32Rect rect = new Int32Rect(x, y, width, height);
            return rect;
        }
    }
}
