using System.Windows;
using System.Windows.Input;

namespace FighterMaker.Visual.Windows
{
    /// <summary>
    /// Interaction logic for NewTextWindow.xaml
    /// </summary>
    public partial class NewTextWindow : Window
    {
        public string SettedValue { get; private set; } = string.Empty;   
        public object LabelContentValue { get => InfoLabel.Content; set => InfoLabel.Content = value; }
        public string TextBoxContentValue { get => ValueTextBox.Text; set=> ValueTextBox.Text = value; }
        public string WindowTitle { get=> this.Title; set => this.Title = value; }

        public NewTextWindow()
        {
            InitializeComponent();
            ValueTextBox.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CallClose();
            else if (e.Key == Key.Escape)
                Close();
        }

        private void CallClose()
        {
            if (string.IsNullOrWhiteSpace(ValueTextBox.Text))
            {
                Close();
                return;
            }

            SettedValue = ValueTextBox.Text.Trim();

            this.DialogResult = true;
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CallClose();
        }
    }
}
