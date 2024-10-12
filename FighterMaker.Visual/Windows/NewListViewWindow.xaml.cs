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
using System.Collections;

namespace FighterMaker.Visual.Windows
{
    /// <summary>
    /// Interaction logic for NewListViewWindow.xaml
    /// </summary>
    public partial class NewListViewWindow : Window
    {
        public object SelectedValue { get => ListValue.SelectedItem; }
        public object LabelContentValue { get => InfoLabel.Content; set => InfoLabel.Content = value; }
        public IEnumerable ListSource { get => ListValue.ItemsSource; set => ListValue.ItemsSource = value; }
        public string WindowTitle { get => this.Title; set => this.Title = value; }

        public NewListViewWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CallClose();
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CallClose();
        }

        public void CallClose()
        {
            DialogResult = SelectedValue != null;
            Close();
        }
    }
}
