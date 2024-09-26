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
                var animationModel = new AnimationModel();
                animationModel.BasicValues.Name = newAnimationWindow.SelectedAnimationName;
                AnimationSequence.SelectedAnimation = animationModel;
                AnimationSequence.NameBoxSelectionChanged += AnimationSequence_NameBoxSelectionChanged;

                var propertiesView = new PropertiesViewControl();
                propertiesView.Width = 400;
                propertiesView.CurrentObject = animationModel;
                propertiesView.Analize();

                propertiesViewControlsMap.Add(animationModel, propertiesView);

                MainExpander.Content = propertiesView;
            }
        }

        private void AnimationSequence_NameBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAnimation = AnimationSequence.SelectedAnimation;

            if (selectedAnimation == null)
                return;

            PropertiesViewControl? propertiesViewControl;

            propertiesViewControlsMap.TryGetValue(selectedAnimation, out propertiesViewControl);

            if(propertiesViewControl != null)
                MainExpander.Content = propertiesViewControl;
        }
    }
}
