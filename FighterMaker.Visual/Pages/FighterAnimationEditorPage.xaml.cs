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
                AnimationSequence.AnimationName = newAnimationWindow.SelectedAnimationName;
            }
        }
    }
}
