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
using System.Windows.Shapes;

namespace FighterMaker.Visual.Windows
{
    /// <summary>
    /// Interaction logic for NewAnimationWindow.xaml
    /// </summary>
    public partial class NewAnimationWindow : Window
    {
        public string SelectedAnimationName { get; private set; } = string.Empty;

        public NewAnimationWindow()
        {
            InitializeComponent();

            AnimationNameBox.Focus();
        }   

        private void CallClose()
        {
            if (string.IsNullOrWhiteSpace(AnimationNameBox.Text))
            {
                Close();
                return;
            }

            SelectedAnimationName = AnimationNameBox.Text.Trim();

            this.DialogResult = true;
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CallClose();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CallClose();
            else if (e.Key == Key.Escape)
                Close();
        }
    }
}
