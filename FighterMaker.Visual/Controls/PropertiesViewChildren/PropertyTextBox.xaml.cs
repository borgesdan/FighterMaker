using FighterMaker.Visual.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;

namespace FighterMaker.Visual.Controls.PropertiesViewChildren
{
    /// <summary>
    /// Interaction logic for PropertyTextBox.xaml
    /// </summary>
    public partial class PropertyTextBox : UserControl
    {
        public PropertyInfo Property { get; protected set; }
        public object PropertyOwner { get; protected set; }

        public object PropertyDisplayContent { get => PropertyContent.Content; set => PropertyContent.Content = value; }
        public string PropertyDisplayText { get => PropertyBox.Text; set => PropertyBox.Text = value; }        

        public PropertyTextBox()
        {
            InitializeComponent();
        }
        
        public PropertyTextBox(object owner, PropertyInfo property) : this() 
        {
            Property = property;
            PropertyOwner = owner;

            Tag = property;
            PropertyDisplayContent = property.Name;
            PropertyDisplayText = Property.GetValueAsString(PropertyOwner);            
        }        

        private void PropertyBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab) 
            {
                Property.SetValue(PropertyOwner, PropertyDisplayText);
            }
            else if(e.Key == Key.Escape)
            {
                PropertyBox.Text = Property.GetValueAsString(PropertyOwner);
            }
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var attribute = Property.GetCustomAttributes(typeof(StringLengthAttribute), false);

            if (attribute != null && attribute.Length > 0)
            {
                var max = ((StringLengthAttribute)attribute[0]).MaximumLength;                
                PropertyBox.MaxLength = max;                
            }                
        }
    }
}
