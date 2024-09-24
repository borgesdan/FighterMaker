using FighterMaker.Visual.Controls.PropertiesViewChildren;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    /// Interaction logic for PropertiesViewControl.xaml
    /// </summary>
    public partial class PropertiesViewControl : UserControl
    {
        public object CurrentObject { get; set; } = null;

        public PropertiesViewControl()
        {
            InitializeComponent();
        }       

        public void Analize()
        {
            if (CurrentObject == null)
                return;

            var type = CurrentObject.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var browsable = property.GetCustomAttributes(typeof(BrowsableAttribute), false);

                if (browsable == null)
                    return;

                if (property.PropertyType == typeof(string))
                {   
                    var textBox = new PropertyTextBox(CurrentObject, property);                    
                    textBox.Height = 28;
                    
                    MainStack.Children.Add(textBox);
                }
            }
        }        
    }
}
