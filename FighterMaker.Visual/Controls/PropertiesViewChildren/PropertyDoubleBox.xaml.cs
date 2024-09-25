using FighterMaker.Visual.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

namespace FighterMaker.Visual.Controls.PropertiesViewChildren
{
    /// <summary>
    /// Interaction logic for PropertyDoubleBox.xaml
    /// </summary>
    public partial class PropertyDoubleBox : UserControl
    {
        bool needLayoutUpdate = true;

        public PropertyInfo Property { get; protected set; }
        public object PropertyOwner { get; protected set; }

        public object PropertyDisplayContent { get => PropertyContent.Content; set => PropertyContent.Content = value; }
        public string PropertyDisplayText { get => PropertyBox.Text; set => PropertyBox.Text = value; }

        public PropertyDoubleBox()
        {
            InitializeComponent();
        }

        public PropertyDoubleBox(object owner, PropertyInfo property) : this()
        {
            Property = property;
            PropertyOwner = owner;

            Tag = property;
            PropertyDisplayContent = property.Name;
        }

        private void SetCurrentPropertyValue()
        {
            PropertyBox.Text = Property.GetValue<double>(PropertyOwner).ToString() ?? "0.0";
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
                    if (double.TryParse(PropertyDisplayText, out double value))
                        Property.SetValue(PropertyOwner, value);
                    else
                        SetCurrentPropertyValue();
                }
            }
            else if (e.Key == Key.Escape)
            {
                SetCurrentPropertyValue();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Valor na textbox
            PropertyDisplayText = Property.GetValue<double>(PropertyOwner).ToString() ?? "0.0";

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
