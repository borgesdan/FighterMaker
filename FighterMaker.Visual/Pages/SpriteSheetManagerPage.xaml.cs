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
using System.Numerics;
using FighterMaker.Visual.Core;
using FighterMaker.Visual.Core.Helpers;
using FighterMaker.Visual.Core.Extensions;

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

        //Flag para previnir a exibição e redimensionamento do retângulo de seleção após abrir arquivo com dois cliques
        bool preventResizeRectangle = false;

        public SpriteSheetManagerPage()
        {
            InitializeComponent();
        }        

        private void HideFrameRectangle()
        {
            CanvasFrameRectangle.Visibility = Visibility.Hidden;
            CanvasFrameRectangle.Width = 0;
            CanvasFrameRectangle.Height = 0;
        }

        private void SetFrameRectanglePosition()
        {
            var imageLeftTop = CanvasImage.GetCanvasLeftTopProperties();

            var left = imageLeftTop.Item1 + selectedRectanglePosition.X;
            var top = imageLeftTop.Item2 + selectedRectanglePosition.Y;

            CanvasFrameRectangle.SetValue(Canvas.LeftProperty, left);
            CanvasFrameRectangle.SetValue(Canvas.TopProperty, top);
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

                preventResizeRectangle = true;

                HideFrameRectangle();
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

            SetFrameRectanglePosition();

            HideFrameRectangle();
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
                if (preventResizeRectangle)
                {
                    preventResizeRectangle = false;
                    return;
                }

                CanvasFrameRectangle.Visibility = Visibility.Visible;
                currentMousePosition = e.GetPosition(CanvasImage);

                var transformValue = currentMousePosition - selectedRectanglePosition;

                var width = Math.Clamp(transformValue.X, 0.0, double.MaxValue);
                var height = Math.Clamp(transformValue.Y, 0.0, double.MaxValue);                

                CanvasFrameRectangle.Width = width;
                CanvasFrameRectangle.Height = height;

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
            var bitmap = CanvasImage.Source as BitmapSource;

            if (bitmap == null)
                return;

            Int32Rect rect;

            rect.X = (int)selectedRectanglePosition.X;
            rect.Y = (int)selectedRectanglePosition.Y;
            rect.Width = (int)CanvasFrameRectangle.Width;
            rect.Height = (int)CanvasFrameRectangle.Height;            

            var bitmapSourceSlice = new BitmapSourceSlice(bitmap, rect);
            var fittedRect = bitmapSourceSlice.GetFittedRectangle();

            var top = (double)CanvasFrameRectangle.GetValue(Canvas.TopProperty);
            var left = (double)CanvasFrameRectangle.GetValue(Canvas.LeftProperty);
            
            CanvasFrameRectangle.SetValue(Canvas.LeftProperty, (double)left + fittedRect.X);
            CanvasFrameRectangle.SetValue(Canvas.TopProperty, (double)top + fittedRect.Y);
            CanvasFrameRectangle.Width = fittedRect.Width - fittedRect.X;
            CanvasFrameRectangle.Height = fittedRect.Height - fittedRect.Y;
        }
    }
}
