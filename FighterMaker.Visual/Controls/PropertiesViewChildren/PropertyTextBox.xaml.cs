using FighterMaker.Visual.Extensions;
using System.ComponentModel;
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
        }      
        
        private void SetCurrentPropertyValue()
        {
            PropertyBox.Text = Property.GetValue<string>(PropertyOwner) ?? string.Empty;
        }

        private void PropertyBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab) 
            {
                if (string.IsNullOrWhiteSpace(PropertyBox.Text))
                {
                    SetCurrentPropertyValue();
                }
                else
                {
                    Property.SetValue(PropertyOwner, PropertyDisplayText);
                }
            }
            else if(e.Key == Key.Escape)
            {
                SetCurrentPropertyValue();
            }
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
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

            //Valor na textbox
            PropertyDisplayText = Property.GetValue<string>(PropertyOwner) ?? string.Empty;
        }
    }
}
