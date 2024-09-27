using FighterMaker.Visual.Controls;
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
        List<AnimationModel> animations = [];

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

        private AnimationModel AddAnimation(string animationName)
        {
            var animationModel = new AnimationModel();
            animationModel.BasicValues.NameChanged += BasicValues_NameChanged;
            animationModel.BasicValues.Name = animationName;
            AnimationSequence.SelectedAnimation = animationModel;
            AnimationSequence.NameBoxSelectionChanged += AnimationSequence_NameBoxSelectionChanged;

            animations.Add(animationModel);

            return animationModel;
        }

        private void BasicValues_NameChanged(object? sender, Core.Events.ValuePropertyChangedEventArgs<string> e)
        {
            if (HasAnimation(e.Value))
            {
                MessageBox.Show("There is already an animation with the given name.");
                return;
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
