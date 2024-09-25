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
        bool needLayoutUpdate = true;

        public PropertyInfo? Property { get; protected set; }

        public object HeaderDisplay { get => MainGroupBox.Header; set => MainGroupBox.Header = value; }
        public UIElementCollection Children { get => MainGrouBoxStack.Children; }

        public PropertyGroupBox()
        {
            InitializeComponent();
        }

        public PropertyGroupBox(PropertyInfo property) : this() 
        {
            Property = property;

            Tag = property;
            HeaderDisplay = property.Name;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!needLayoutUpdate)
                return;            

            if (Property == null)
                throw new NullReferenceException();            

            //Nome da sessão ser adicionado no Header
            var displayName = Property.SelectAttribute<DisplayNameAttribute>();

            if (displayName != null)
                HeaderDisplay = displayName.DisplayName;

            //Descrição a ser exibida em um tooltip
            var description = Property.SelectAttribute<DescriptionAttribute>();

            if (description != null)
                ToolTip = description.Description;

            needLayoutUpdate = false;
        }
    }
}
