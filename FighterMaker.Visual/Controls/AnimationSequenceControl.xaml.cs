using FighterMaker.Visual.Core;
using FighterMaker.Visual.Core.Events;
using FighterMaker.Visual.Models;
using FighterMaker.Visual.Pages;
using System;
using System.Collections.Generic;
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
using FighterMaker.Visual.Core.Extensions;
using System.Diagnostics;

namespace FighterMaker.Visual.Controls
{
    /// <summary>
    /// Represents a control that displays an animation selection and its frames
    /// </summary>
    public partial class AnimationSequenceControl : UserControl
    {
        AnimationModelCollection animations = [];

        /// <summary> Gets the selected animation. </summary>
        public AnimationModel? SelectedAnimation => NameBox.SelectedItem as AnimationModel;

        /// <summary>Event to be executed when the add animation button was clicked.</summary>
        public event RoutedEventHandler? AddAnimationButtonClick;
        /// <summary>Event to be executed the selected animation is changed</summary>
        public event SelectionChangedEventHandler? NameBoxSelectionChanged;
        /// <summary>Occurs when the selection of an animation frame is changed.</summary>
        public event EventHandler<BitmapSourceSlice?>? FrameSelectionChanged;
        /// <summary>Occurs when a frame of an animation has its value replaced.</summary>
        public event EventHandler<BitmapSourceSlice?>? FrameValueReplaced;

        public AnimationSequenceControl()
        {
            InitializeComponent();
        }

        public void SetCollection(AnimationModelCollection collection)
        {
            animations = collection;
            animations.ItemAdded += (sender, e) =>
            {
                NameBox.ItemsSource = animations;
                NameBox.SelectedIndex = animations.Count - 1;
                e.BasicValues.EndNameChanged -= AnimationNameChanged;
                e.BasicValues.EndNameChanged += AnimationNameChanged;
            };
            
            NameBox.ItemsSource = animations;

            if(animations.Count > 0)
            {
                NameBox.SelectedIndex = 0;

                foreach (var item in animations)
                {
                    item.BasicValues.EndNameChanged -= AnimationNameChanged;
                    item.BasicValues.EndNameChanged += AnimationNameChanged;
                }                    
            }
        }

        private void AnimationNameChanged(object? sender, string e)
        {
            this.UpdateLayout();

            var selected = NameBox.SelectedItem;
            NameBox.SelectedItem = null;
            NameBox.SelectedItem = selected;
        }

        private void AddAnimationButton_Click(object sender, RoutedEventArgs e) => AddAnimationButtonClick?.Invoke(sender, e);

        private void NameBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NameBoxSelectionChanged?.Invoke(sender, e);

            FrameListView.Items.Clear();

            if (SelectedAnimation == null)
            {
                FrameListView.SelectedIndex = -1;
                return;
            }

            foreach(var frame in SelectedAnimation.Frames)
            {
                Debug.Assert(frame is not null && frame.SourceTexture is not null);

                var slice = new BitmapSourceSlice(frame.SourceTexture, frame.Bounds);
                var rectangle = CreateFrameViewRectangle(Brushes.Green, slice);
                FrameListView.Items.Add(rectangle);
            }         
            
            FrameListView.SelectedIndex = 0;
        }        

        private void OpenSpriteSheetButton_Click(object sender, RoutedEventArgs e)
        {
            var sheetManagerPage = new SpriteSheetManagerPage();

            var frame = new Frame
            {
                Content = sheetManagerPage
            };

            var window = new Window
            {
                WindowStyle = WindowStyle.SingleBorderWindow
            };

            sheetManagerPage.InsertAfterFrameButtonClick += SheetManagerPage_InsertAfterFrameButtonClick;
            sheetManagerPage.InsertBeforeFrameButtonClick += SheetManagerPage_InsertBeforeFrameButtonClick;
            sheetManagerPage.ReplaceFrameButtonClick += SheetManagerPage_ReplaceFrameButtonClick;
            window.Content = frame;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Owner = App.Current.MainWindow;

            window.Show();
        }        

        private void SheetManagerPage_InsertBeforeFrameButtonClick(object? sender, SpriteSheetEventArgs e)
        {
            var frame = InsertFrame(e, true);

            Debug.Assert(frame is not null && frame.BitmapSlice is not null, "Frame or BitmapSlice is null");

            if (frame is null)
                return;
            
            var model = new AnimationFrameModel
            {
                SourceTexture = frame?.BitmapSlice?.Source,
                Bounds = frame?.BitmapSlice != null ? frame.BitmapSlice.SourceRectangle : new Int32Rect()
            };

            SelectedAnimation?.Frames.Insert(frame!.Index, model);
            
            UpdateModel(frame);
        }

        private void SheetManagerPage_InsertAfterFrameButtonClick(object? sender, SpriteSheetEventArgs e)
        {
            var frame = InsertFrame(e);           

            UpdateModel(frame);
        }

        private void SheetManagerPage_ReplaceFrameButtonClick(object? sender, SpriteSheetEventArgs e)
        {
            var frame = ReplaceFrame(e);            

            UpdateModel(frame);

            FrameValueReplaced?.Invoke(sender, e.FrameSource);
        }

        private void FrameListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FrameListView.SelectedItem is not Rectangle selectedItem)
                return;

            var bitmap = selectedItem.Tag as BitmapSourceSlice;

            FrameSelectionChanged?.Invoke(sender, bitmap);
        }

        private SelectedFrame? InsertFrame(SpriteSheetEventArgs e, bool before = false)
        {
            if (e.FrameSource == null)
                return null;

            if (NameBox.SelectedItem == null)
                return null;
            
            var rectangle = CreateFrameViewRectangle(Brushes.Green, e.FrameSource);

            int objIndex;
            var currentSelectedIndex = FrameListView.SelectedIndex;

            if (currentSelectedIndex > -1)
            {
                var insertIndex = before ? currentSelectedIndex : currentSelectedIndex + 1;

                rectangle.Fill = Brushes.Blue;
                FrameListView.Items.Insert(insertIndex, rectangle);

                objIndex = insertIndex;
            }
            else
            {
                objIndex = FrameListView.Items.Add(rectangle);
            }

            FrameListView.SelectedIndex = objIndex;

            var insertedFrame = new SelectedFrame
            {
                Index = objIndex,
                BitmapSlice = e.FrameSource,
            };

            return insertedFrame;
        }

        private SelectedFrame? ReplaceFrame(SpriteSheetEventArgs e)
        {
            if (e.FrameSource == null)
                return null;

            if (NameBox.SelectedItem == null)
                return null;

            var currentSelectedIndex = FrameListView.SelectedIndex;

            if(currentSelectedIndex < 0)
            {
                return InsertFrame(e);                
            }


            if (FrameListView.SelectedItem is Rectangle selectedItem)
                selectedItem.Fill = Brushes.Orange;

            var replacedFrame = new SelectedFrame
            {
                Index = currentSelectedIndex,
                BitmapSlice = e.FrameSource,
                Replace = true
            };

            return replacedFrame;
        }

        private void UpdateModel(SelectedFrame? selectedFrame)
        {
            if (selectedFrame == null || selectedFrame.BitmapSlice == null)
                return;            

            if (selectedFrame.Replace)
            {
                try
                {
                    var frame = (SelectedAnimation?.Frames[selectedFrame.Index]) ?? throw new InvalidOperationException();
                    frame.SourceTexture = selectedFrame.BitmapSlice.Source;
                    frame.Bounds = selectedFrame.BitmapSlice.SourceRectangle;

                } catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                var frameModel = new AnimationFrameModel
                {
                    SourceTexture = selectedFrame.BitmapSlice.Source,
                    Bounds = selectedFrame.BitmapSlice.SourceRectangle
                };

                SelectedAnimation?.Frames.Insert(selectedFrame.Index, frameModel);
            }            
        }

        private static Rectangle CreateFrameViewRectangle(Brush brush, BitmapSourceSlice? bitmapSourceSlice)
        {
            var rectangle = new Rectangle
            {
                Width = 10,
                Height = 10,
                Fill = brush,
                Tag = bitmapSourceSlice
            };

            return rectangle;
        }
    }

    public class SelectedFrame
    {
        public int Index { get; set; }
        public BitmapSourceSlice? BitmapSlice { get; set; }
        public bool Replace { get; set; }
    }
}
