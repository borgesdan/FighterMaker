using FighterMaker.Visual.Core;
using FighterMaker.Visual.Core.Extensions.Models;
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
    /// Interaction logic for ActorManagerPage.xaml
    /// </summary>
    public partial class ActorManagerPage : Page
    {
        private readonly ActorModelCollection actors = [];

        public ActorModel? CurrentActor { get; set; }

        public ActorManagerPage()
        {
            InitializeComponent();
        }

        private void AddActorButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewAnimationWindow();
            var dialog = window.ShowDialog();

            if (dialog != null && dialog.Value)
            {
                var model = new ActorModel() { Name = window.SelectedAnimationName };                

                actors.Add(model);
                CurrentActor = model;

                AnimationEditor.SetActor(model);
            }
        }
    }
}
