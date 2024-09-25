using FighterMaker.Visual.Core.Extensions;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for PropertyGroupBox.xaml
    /// </summary>
    public partial class PropertyGroupBox : UserControl
    {
        public PropertyInfo Property { get; protected set; }
        public object PropertyOwner { get; protected set; }

        public object HeaderDisplay { get => MainGroupBox.Header; set => MainGroupBox.Header = value; }
        public UIElementCollection Children { get => MainGrouBoxStack.Children; }

        public PropertyGroupBox()
        {
            InitializeComponent();
        }

        public PropertyGroupBox(object owner, PropertyInfo property) : this() 
        {
            Property = property;
            PropertyOwner = owner;

            Tag = property;
            HeaderDisplay = property.Name;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Valor na label
            var displayName = Property.SelectAttribute<DisplayNameAttribute>();

            if (displayName != null)
                HeaderDisplay = displayName;
        }
    }
}
