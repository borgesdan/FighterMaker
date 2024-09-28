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
using FighterMaker.Visual.Core.Events;

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

        Int32Rect selectedFrameRectangle;
        BitmapSourceSlice? selectedBitmapSourceSlice;
        CroppedBitmap? selectedCroppedBitmapSouce;

        //Flag to prevent displaying and resizing selection rectangle
        //after double-clicking file open
        bool preventResizeRectangle = false;

        public event EventHandler<SpriteSheetEventArgs>? InsertAfterFrameButtonClick;
        public event EventHandler<SpriteSheetEventArgs>? InsertBeforeFrameButtonClick;
        public event EventHandler<SpriteSheetEventArgs>? ReplaceFrameButtonClick;

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
                
                try
                {
                    AdjustRectangle();
                }
                catch
                {
                    MessageBox.Show("Operação inválida");
                }
            }
        }       

        private void InsertAfterFrameButton_Click(object sender, RoutedEventArgs e)
        {
            var args = new SpriteSheetEventArgs(selectedBitmapSourceSlice);
            InsertAfterFrameButtonClick?.Invoke(sender, args);
        }

        private void InsertBeforeFrameButton_Click(object sender, RoutedEventArgs e)
        {
            var args = new SpriteSheetEventArgs(selectedBitmapSourceSlice);
            InsertBeforeFrameButtonClick?.Invoke(sender, args);
        }

        private void ReplaceFrameButton_Click(object sender, RoutedEventArgs e)
        {
            var args = new SpriteSheetEventArgs(selectedBitmapSourceSlice);
            ReplaceFrameButtonClick?.Invoke(sender, args);
        }

        private void AdjustRectangle()
        {
            if (CanvasImage.Source is not BitmapSource bitmap)
                return;

            Int32Rect rect;
            rect.X = (int)selectedRectanglePosition.X;
            rect.Y = (int)selectedRectanglePosition.Y;
            rect.Width = (int)CanvasFrameRectangle.Width;
            rect.Height = (int)CanvasFrameRectangle.Height;

            var bitmapSourceSlice = new BitmapSourceSlice(bitmap, rect);
            var fittedRect = bitmapSourceSlice.GetFittedRectangle();            

            AdjustCanvasFrameRectangleFrom(fittedRect);

            selectedFrameRectangle = rect;
            selectedFrameRectangle.X += fittedRect.X;
            selectedFrameRectangle.Y += fittedRect.Y;
            selectedFrameRectangle.Width = fittedRect.Width - fittedRect.X;
            selectedFrameRectangle.Height = fittedRect.Height - fittedRect.Y;

            selectedBitmapSourceSlice = new BitmapSourceSlice(bitmap, selectedFrameRectangle);
        }        

        private void AdjustCanvasFrameRectangleFrom(Int32Rect rectangle)
        {
            var top = (double)CanvasFrameRectangle.GetValue(Canvas.TopProperty);
            var left = (double)CanvasFrameRectangle.GetValue(Canvas.LeftProperty);

            SetCanvasFrameRectangle(
                (double)left + rectangle.X,
                (double)top + rectangle.Y,
                rectangle.Width - rectangle.X,
                rectangle.Height - rectangle.Y);
        }

        private void SetCanvasFrameRectangle(double x, double y, double width, double height)
        {
            CanvasFrameRectangle.SetValue(Canvas.LeftProperty, x);
            CanvasFrameRectangle.SetValue(Canvas.TopProperty, y);
            CanvasFrameRectangle.Width = width;
            CanvasFrameRectangle.Height = height;
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
    }      
}
