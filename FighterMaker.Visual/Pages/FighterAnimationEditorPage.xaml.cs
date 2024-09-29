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
    public partial class FighterAnimationEditorPage : Page
    {
        AnimationModelCollection animations = [];

        private Dictionary<AnimationModel, PropertiesViewControl> propertiesViewControlsMap = [];

        public FighterAnimationEditorPage()
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

        private void AnimationSequence_FrameSelected(object sender, Rectangle? e)
        {
            if (e == null)
            {
                CanvasImage.Source = null;
                return;
            }

            var source = e.Tag as BitmapSourceSlice;

            if (source == null)
                return;

            CanvasImage.Source = source.Cropped;
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
    }
}
