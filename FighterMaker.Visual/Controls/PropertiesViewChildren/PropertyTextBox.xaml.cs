using FighterMaker.Visual.Core;
using FighterMaker.Visual.Core.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Resources;

namespace FighterMaker.Visual.Controls.PropertiesViewChildren
{
    /// <summary>
    /// Interaction logic for PropertyTextBox.xaml
    /// </summary>
    public partial class PropertyTextBox : UserControl
    {
        bool needLayoutUpdate = true;

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
        }      
        
        private void SetCurrentPropertyValue()
        {
            PropertyBox.Text = Property.GetValue<string>(PropertyOwner) ?? string.Empty;
        }

        private void PropertyBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab) 
            {
                if (!string.IsNullOrWhiteSpace(PropertyBox.Text))
                {
                    Property.SetValue(PropertyOwner, PropertyDisplayText);

                    var value = Property.GetValue<string>(PropertyOwner);
                    
                    if (!PropertyDisplayText.Equals(value))
                    {                        
                        MessageBox.Show("There is already an animation with the given name.");
                    }
                }

                SetCurrentPropertyValue();
            }
            else if(e.Key == Key.Escape)
            {
                SetCurrentPropertyValue();
            }
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //Valor na textbox
            PropertyDisplayText = Property.GetValue<string>(PropertyOwner) ?? string.Empty;

            if (!needLayoutUpdate)
                return;            

            //Valor na label
            var displayName = Property.SelectAttribute<DisplayNameAttribute>();

            if (displayName != null)
                PropertyDisplayContent = displayName.DisplayName;            

            //Tamanho da string na textbox
            var stringLength = Property.SelectAttribute<StringLengthAttribute>();

            if (stringLength != null && stringLength.MaximumLength > 0)
                PropertyBox.MaxLength = stringLength.MaximumLength;

            //Descrição a ser exibida em um tooltip
            var description = Property.SelectAttribute<DescriptionAttribute>();

            if (description != null)
                ToolTip = description.Description;

            needLayoutUpdate = false;
        }
    }
}
