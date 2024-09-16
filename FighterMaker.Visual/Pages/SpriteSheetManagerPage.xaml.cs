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

namespace FighterMaker.Visual.Pages
{
    /// <summary>
    /// Interaction logic for SpriteSheetManagerPage.xaml
    /// </summary>
    public partial class SpriteSheetManagerPage : Page
    {
        Point oldMousePosition;
        Point currentMousePosition;
        Vector2 currentImageCanvasOffset;
        
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
            oldMousePosition = currentMousePosition;
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

                var transformValue = currentMousePosition - oldMousePosition;

                var finalX = Math.Clamp(transformValue.X, 0.0, double.MaxValue);
                var finalY = Math.Clamp(transformValue.Y, 0.0, double.MaxValue);

                var imageLeftTop = CanvasImage.GetCanvasLeftTopProperties();

                CanvasFrameRectangle.SetValue(Canvas.LeftProperty, imageLeftTop.Item1 + oldMousePosition.X);
                CanvasFrameRectangle.SetValue(Canvas.TopProperty, imageLeftTop.Item2 + oldMousePosition.Y);

                CanvasFrameRectangle.Width = finalX;
                CanvasFrameRectangle.Height = finalY;
            }
        }
    }
}
