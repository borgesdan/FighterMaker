﻿using FighterMaker.Visual.Core.Events;
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

namespace FighterMaker.Visual.Controls
{
    /// <summary>
    /// Represents a control that displays an animation selection and its frames
    /// </summary>
    public partial class AnimationSequenceControl : UserControl
    {
        /// <summary>
        /// Gets or sets the selected animation.
        /// </summary>
        public AnimationModel? SelectedAnimation
        {
            get
            {
                var selected = NameBox.SelectedItem as ComboBoxItem;
                return selected?.Content as AnimationModel;
            }
            set
            {
                if (value == null)
                    return;

                var item = new ComboBoxItem
                {
                    Content = value,
                    IsSelected = true
                };

                value.BasicValues.EndNameChanged += AnimationModel_EndNameChanged;

                NameBox.Items.Add(item);
            }
        }

        /// <summary>Event to be executed when the add animation button was clicked.</summary>
        public event RoutedEventHandler? AddAnimationButtonClick;
        /// <summary>Event to be executed the selected animation is changed</summary>
        public event SelectionChangedEventHandler? NameBoxSelectionChanged;
        /// <summary>Occurs when the selection of an animation frame is changed.</summary>
        public event EventHandler<Rectangle?>? FrameSelectionChanged;

        public AnimationSequenceControl()
        {
            InitializeComponent();
        }

        private void AddAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            AddAnimationButtonClick?.Invoke(sender, e);
        }

        private void NameBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NameBoxSelectionChanged?.Invoke(sender, e);
        }

        //Forces the control to update if the animation name changes
        private void AnimationModel_EndNameChanged(object? sender, string e)
        {
            this.UpdateLayout();

            var selected = NameBox.SelectedItem;
            NameBox.SelectedItem = null;
            NameBox.SelectedItem = selected;
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
            window.Content = frame;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Owner = App.Current.MainWindow;

            window.Show();
        }

        private void SheetManagerPage_InsertBeforeFrameButtonClick(object? sender, SpriteSheetEventArgs e)
        {
            InsertFrame(e, true);
        }

        private void SheetManagerPage_InsertAfterFrameButtonClick(object? sender, SpriteSheetEventArgs e)
        {
            InsertFrame(e, true);
        }

        private Rectangle? InsertFrame(SpriteSheetEventArgs e, bool before = false)
        {
            if (e.IsEmpty)
                return null;

            if (NameBox.SelectedItem == null)
                return null;

            Rectangle rectangle = new Rectangle();
            rectangle.Width = 20;
            rectangle.Height = 20;
            rectangle.Fill = Brushes.Green;
            rectangle.Tag = e.FrameSource;

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

            return rectangle;
        }

        private void FrameListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = FrameListView.SelectedItem as Rectangle;
            FrameSelectionChanged?.Invoke(sender, selectedItem);
        }
    }
}
