using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FighterMaker.Visual.Extensions;
using System.Numerics;
using FighterMaker.Visual.Core;
using FighterMaker.Visual.Helpers;

namespace FighterMaker.Visual.Pages
{
    /// <summary>
    /// Interaction logic for SpriteSheetManagerPage.xaml
    /// </summary>
    public partial class SpriteSheetManagerPage : Page
    {
        Point selectedRectanglePosition;
        Point oldMousePosition;
        Point currentMousePosition;
        Vector2 currentImageCanvasOffset;
        MouseButtonState oldLeftButtonState;
        BitmapDecoder? bitmapDecoder = null;
        Stream imageStream;

        public SpriteSheetManagerPage()
        {
            InitializeComponent();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var dialogResult = openFileDialog.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value)
            {
                var uri = new Uri(openFileDialog.FileName);
                var bitmap = new BitmapImage(uri);

                CanvasImage.ResetCanvasTopLeftProperties();
                CanvasImage.Source = bitmap;

                imageStream = openFileDialog.OpenFile();

                //bitmapDecoder = PngBitmapDecoder.Create(imageStream, BitmapCreateOptions.None, BitmapCacheOption.Default);
            }
        }

        private void CanvasImage_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            currentMousePosition = e.GetPosition(MainCanvas);
            oldMousePosition = currentMousePosition;

            var leftTop = CanvasImage.GetCanvasLeftTopProperties();
            currentImageCanvasOffset = new Vector2((float)leftTop.Item1, (float)leftTop.Item2);            
        }

        private void CanvasImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentMousePosition = e.GetPosition(CanvasImage);
            selectedRectanglePosition = currentMousePosition;

            CanvasFrameRectangle.Visibility = Visibility.Hidden;
            CanvasFrameRectangle.Width = 0;
            CanvasFrameRectangle.Height = 0;
        }

        private void CanvasImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                currentMousePosition = e.GetPosition(MainCanvas);
                var transformValue = currentMousePosition - oldMousePosition;

                var finalX = transformValue.X + currentImageCanvasOffset.X;
                var finalY = transformValue.Y + currentImageCanvasOffset.Y;

                CanvasImage.SetValue(Canvas.LeftProperty, finalX);
                CanvasImage.SetValue(Canvas.TopProperty, finalY);

            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                CanvasFrameRectangle.Visibility = Visibility.Visible;
                currentMousePosition = e.GetPosition(CanvasImage);

                var transformValue = currentMousePosition - selectedRectanglePosition;

                var finalX = Math.Clamp(transformValue.X, 0.0, double.MaxValue);
                var finalY = Math.Clamp(transformValue.Y, 0.0, double.MaxValue);

                var imageLeftTop = CanvasImage.GetCanvasLeftTopProperties();

                var left = imageLeftTop.Item1 + selectedRectanglePosition.X;
                var top = imageLeftTop.Item2 + selectedRectanglePosition.Y;

                CanvasFrameRectangle.SetValue(Canvas.LeftProperty, left);
                CanvasFrameRectangle.SetValue(Canvas.TopProperty, top);

                CanvasFrameRectangle.Width = finalX;
                CanvasFrameRectangle.Height = finalY;

                oldLeftButtonState = e.LeftButton;
            }

            if (oldLeftButtonState == MouseButtonState.Pressed && e.LeftButton == MouseButtonState.Released)
            {
                oldLeftButtonState = MouseButtonState.Released;
                AdjustRectangle();
            }
        }


        private void AdjustRectangle()
        {
            //https://stackoverflow.com/questions/8311805/bitmapsource-copypixels-byte-bitmapsource-how-to-do-this-simple
            //https://medium.com/@oleg.shipitko/what-does-stride-mean-in-image-processing-bba158a72bcd
            //https://stackoverflow.com/questions/16220472/how-to-create-a-bitmapimage-from-a-pixel-byte-array-live-video-display
            //https://stackoverflow.com/questions/14876989/how-to-read-pixels-in-four-corners-of-a-bitmapsource

            var bitmap = CanvasImage.Source as BitmapSource;

            if (bitmap == null)
                return;

            Int32Rect rect;

            rect.X = (int)selectedRectanglePosition.X;
            rect.Y = (int)selectedRectanglePosition.Y;
            rect.Width = (int)CanvasFrameRectangle.Width;
            rect.Height = (int)CanvasFrameRectangle.Height;            

            var stride = (bitmap.Format.BitsPerPixel / 8);
            int[] pixels = new int[rect.Height * rect.Width];            
            bitmap.CopyPixels(rect, pixels, stride * rect.Width, 0);
           
            var y = 0;
            var x = 0;
            var width = 0;
            var height = 0;

            //Definimos o array de pixels em linhas e colunas
            for (int col = 0; col < rect.Width; ++col)
            {
                for (int row = 0; row < rect.Height; ++row)
                {
                    //Verificamos os pixels adjacentes e fazemos comparações

                    var currentPixel = pixels[col + (row * rect.Width)];

                    if (col + 1 < rect.Width)
                    {
                        var rightPixel = pixels[col + 1 + (row * rect.Width)];

                        if (currentPixel != rightPixel)
                        {                            
                            if (x == 0 || col + 1 < x)  //Encontramos o limite esquerdo
                                x = col + 1;
                            else if (col + 1 > width) //Encontramos o limite direito
                                width = col + 1;
                        }
                    }

                    if (row + 1 < rect.Height)
                    {
                        var bottomPixel = pixels[col + ((row + 1) * rect.Width)];

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

            var top = (double)CanvasFrameRectangle.GetValue(Canvas.TopProperty);
            var left = (double)CanvasFrameRectangle.GetValue(Canvas.LeftProperty);
            
            CanvasFrameRectangle.SetValue(Canvas.TopProperty, (double)top + y);
            CanvasFrameRectangle.SetValue(Canvas.LeftProperty, (double)left + x);
            CanvasFrameRectangle.Width = width - x;
            CanvasFrameRectangle.Height = height - y;
        }
    }
}
