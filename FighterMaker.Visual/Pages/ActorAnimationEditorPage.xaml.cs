using FighterMaker.Visual.Controls;
using FighterMaker.Visual.Core;
using FighterMaker.Visual.Models;
using FighterMaker.Visual.Windows;
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

namespace FighterMaker.Visual.Pages
{
    /// <summary>
    /// Interaction logic for FighterAnimationEditorPage.xaml
    /// </summary>
    public partial class ActorAnimationEditorPage : Page
    {
        AnimationModelCollection animations = [];

        private Dictionary<AnimationModel, PropertiesViewControl> propertiesViewControlsMap = [];

        public ActorAnimationEditorPage()
        {
            InitializeComponent();
        }        

        private void AnimationSequence_AddAnimationButtonClick(object sender, RoutedEventArgs e)
        {
            NewAnimationWindow newAnimationWindow = new NewAnimationWindow();
            var dialogResult = newAnimationWindow.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value == true)
            {
                var animationModel = AddAnimation(newAnimationWindow.SelectedAnimationName);

                if (animationModel == null)
                    return;

                var propertiesView = AddPropertiesView(animationModel);

                MainExpander.Content = propertiesView;
            }
        }

        private void AnimationSequence_NameBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAnimation = AnimationSequence.SelectedAnimation;

            if (selectedAnimation == null)
                return;            

            propertiesViewControlsMap.TryGetValue(selectedAnimation, out PropertiesViewControl? propertiesViewControl);

            if(propertiesViewControl != null)
                MainExpander.Content = propertiesViewControl;
        }

        private void AnimationSequence_FrameSelected(object sender, BitmapSourceSlice? e)
        {
            if (e == null)
            {
                ResetCanvasObjects();
                return;
            }

            SetCanvasObjects(e.Cropped);
        }

        private void AnimationSequence_FrameValueReplaced(object sender, BitmapSourceSlice e)
        {
            if (e == null)
            {
                ResetCanvasObjects();
                return;
            }

            SetCanvasObjects(e.Cropped);            
        }

        private AnimationModel? AddAnimation(string animationName)
        {
            try
            {
                var animationModel = animations.Add(animationName);

                AnimationSequence.SelectedAnimation = animationModel;
                AnimationSequence.NameBoxSelectionChanged += AnimationSequence_NameBoxSelectionChanged;                

                return animationModel;

            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return null;
            }            
        }        

        private PropertiesViewControl AddPropertiesView(AnimationModel animationModel)
        {
            var propertiesView = new PropertiesViewControl
            {
                Width = 400,
                CurrentObject = animationModel
            };

            propertiesView.Analize();
            propertiesViewControlsMap.Add(animationModel, propertiesView);

            return propertiesView;
        }

        private bool HasAnimation(string? animationName)
        {
            return animations.Any(x => x.BasicValues.Name == animationName);
        }        

        private void ResetCanvasObjects() 
        {
            CanvasVerticalLine.Visibility = Visibility.Collapsed;
            CanvasHorizontalLine.Visibility = Visibility.Collapsed;
            CanvasImage.Source = null;
        }

        private void SetCanvasObjects(BitmapSource bitmap)
        {
            CanvasImage.Source = bitmap;            
            CanvasImage.RenderTransform = new TranslateTransform((MainCanvas.RenderSize.Width / 2) - bitmap.Width / 2, (MainCanvas.RenderSize.Height / 2) - bitmap.Height / 2);

            CanvasVerticalLine.Visibility = Visibility.Visible;
            CanvasVerticalLine.Height = MainCanvas.RenderSize.Height;            
            var x = (double)CanvasImage.RenderTransform.GetValue(TranslateTransform.XProperty);
            CanvasVerticalLine.RenderTransform = new TranslateTransform(x + bitmap.Width / 2, 0);

            CanvasHorizontalLine.Visibility = Visibility.Visible;
            CanvasHorizontalLine.Width = MainCanvas.RenderSize.Width;            
            var y = (double)CanvasImage.RenderTransform.GetValue(TranslateTransform.YProperty);
            CanvasHorizontalLine.RenderTransform = new TranslateTransform(0, y + bitmap.Height);
        }
    }
}
