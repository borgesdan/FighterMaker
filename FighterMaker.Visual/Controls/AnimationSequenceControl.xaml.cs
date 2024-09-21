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

namespace FighterMaker.Visual.Controls
{
    /// <summary>
    /// Interaction logic for AnimationSequenceControl.xaml
    /// </summary>
    public partial class AnimationSequenceControl : UserControl
    {
        private string animationName = string.Empty;
        public string AnimationName 
        {
            get => animationName;
            set
            {
                animationName = value.Trim();
                
                var item = new ComboBoxItem();
                item.Content = animationName;
                item.IsSelected = true;                    

                NameBox.Items.Add(item);                
            }
        }

        public object SelectedNameBoxItem { get => NameBox.SelectedItem; }
        
        public event RoutedEventHandler AddAnimationButtonClick;

        public AnimationSequenceControl()
        {            
            InitializeComponent();
        }        

        private void AddAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            AddAnimationButtonClick?.Invoke(sender, e);
        }
    }
}
