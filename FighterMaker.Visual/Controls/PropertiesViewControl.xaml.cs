using FighterMaker.Visual.Controls.PropertiesViewChildren;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
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
        private static readonly Type[] SupportedTypes =
        [
            typeof(string),
        ];
        
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
                if(property == null)
                    continue;

                InternalAnalize(CurrentObject, property, null);
            }
        }    
        
        private void InternalAnalize(object owner, PropertyInfo property, PropertyGroupBox? currentGroupBox)
        {            
            var propertyType = property.PropertyType;

            if (PropertiesViewControl.SupportedTypes.Contains(propertyType))
            {
                UIElement uIElement = null;

                if (propertyType == typeof(string))
                {
                    var textBox = new PropertyTextBox(owner, property);
                    textBox.Height = 28; 
                    uIElement = textBox;
                }

                if (uIElement == null)
                    throw new NullReferenceException();

                if (currentGroupBox != null)
                    currentGroupBox.Children.Add(uIElement);
                else
                    MainStack.Children.Add(uIElement);

                return;
            }

            if (currentGroupBox == null)
            {
                var groupBox = new PropertyGroupBox(owner, property);
                
                MainStack.Children.Add(groupBox);

                var currentOwner = property.GetValue(owner, null);

                if (currentOwner == null)
                    return;

                var type = currentOwner.GetType();
                var properties = type.GetProperties();

                foreach(var p in properties)
                {
                    if(p == null)
                        continue;

                    InternalAnalize(currentOwner, p, groupBox);
                }
            }
        }        
    }
}
