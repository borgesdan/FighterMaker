using FighterMaker.Visual.Controls;
using FighterMaker.Visual.Models;
using FighterMaker.Visual.Pages;
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

namespace FighterMaker.Visual.Plugins.Animation
{
    /// <summary>
    /// Interaction logic for PluginPage.xaml
    /// </summary>
    public partial class PluginPage : Page
    {
        Dictionary<AnimationModel, PropertiesViewControl> propertiesViewControlsMap = [];

        public ActorModel Model { get; private set; } = new();

        public PluginPage()
        {
            InitializeComponent();

            AnimationSequence.NameBoxSelectionChanged += AnimationSequence_NameBoxSelectionChanged;
        }

        public void SetModel(ActorModel model)
        {
            Model = model;            
            //AnimationSequence.SetCollection(model.Animations);
            ResetCanvasObjects();
        }

        private void ResetCanvasObjects() 
        {
            CanvasVerticalLine.Visibility = Visibility.Collapsed;
            CanvasHorizontalLine.Visibility = Visibility.Collapsed;
            CanvasImage.Source = null;
        }

        private void AnimationSequence_NameBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAnimation = AnimationSequence.SelectedAnimation;

            if (selectedAnimation == null)
                return;

            var propertiesViewControl = AddOrGetPropertiesViewControl(selectedAnimation);

            MainExpander.Content = propertiesViewControl;
        }

        private PropertiesViewControl AddOrGetPropertiesViewControl(AnimationModel animationModel)
        {
            propertiesViewControlsMap.TryGetValue(animationModel, out PropertiesViewControl? propertiesViewControl);

            propertiesViewControl ??= AddPropertiesView(animationModel);

            return propertiesViewControl;
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

        private void AnimationSequence_AddAnimationButtonClick(object sender, RoutedEventArgs e)
        {
            NewAnimationWindow newAnimationWindow = new NewAnimationWindow();
            var dialogResult = newAnimationWindow.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value == true)
            {
                var animationModel = AddAnimation(newAnimationWindow.SelectedAnimationName);

                if (animationModel == null)
                    return;

                ResetCanvasObjects();

                var propertiesView = AddOrGetPropertiesViewControl(animationModel);

                MainExpander.Content = propertiesView;
            }
        }

        private AnimationModel? AddAnimation(string animationName)
        {
            try
            {
                //var animationModel = Model.Animations.Add(animationName);
                //return animationModel;
                return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void AnimationSequence_FrameSelectionChanged(object sender, Core.BitmapSourceSlice e)
        {
            if (e == null)
            {
                ResetCanvasObjects();
                return;
            }

            SetCanvasObjects(e.Cropped);
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

        private void AnimationSequence_FrameValueReplaced(object sender, Core.BitmapSourceSlice e)
        {
            if (e == null)
            {
                ResetCanvasObjects();
                return;
            }

            SetCanvasObjects(e.Cropped);
        }
    }
}
