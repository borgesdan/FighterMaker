using FighterMaker.Visual.Controls.PropertiesControlChildren;
using FighterMaker.Visual.Controls.PropertiesViewChildren;
using FighterMaker.Visual.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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

namespace FighterMaker.Visual.Controls
{
    /// <summary>
    /// Interaction logic for PropertiesControl.xaml
    /// </summary>
    public partial class PropertiesControl : UserControl
    {
        private class GroupItem
        {
            public PropertyInfo Source { get; set; } = null!;
            public string Name { get; set; } = string.Empty;
            public object Value { get; set; } = null!;
        }

        private class Group
        {
            public string Name { get; private set; }
            public List<GroupItem> Items { get; private set; } = [];

            public Group(string name)
            {
                Name = name;
            }
        }

        private object? model;
        private List<Group> groups = [];
        static readonly string DefaultGroupName = "Misc";

        static readonly Type[] TextBoxTypes = new[]
        {
            typeof(string),
            typeof(char),
            typeof(byte),
            typeof(sbyte),
            typeof(double),
            typeof(float),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
        };        

        public PropertiesControl()
        {
            InitializeComponent();
        }

        public void SetModel(object model)
        {
            this.model = model;
        }

        public void AnalizeModel()
        {
            if (model == null)
                throw new InvalidOperationException();

            var properties = model.GetType().GetProperties();

            foreach (var property in properties) 
            {
                if (property is null || !property.CanRead)
                    continue;

                var propValue = property.GetValue(model);

                if (propValue == null)
                    continue;

                var propValueType = propValue.GetType();

                if (!propValueType.IsValueType && propValueType != typeof(string))
                {
                    if ((propValueType.IsArray || propValueType.IsGenericList()))
                    {
                        var list = propValue as ICollection;

                        if (list == null) continue;

                        foreach(var listItem in list)
                        {
                            if (!listItem.GetType().IsValueType)
                                break;

                            AddItemInDefaultGroup(new GroupItem { Source = property, Name = property.Name, Value = listItem });
                        }
                    }

                    continue;
                }

                AddItemInDefaultGroup(new GroupItem { Source = property, Name = property.Name, Value = propValue});
            }
        }

        public void PopulateControl()
        {
            foreach(var group in groups)
            {
                if (!group.Items.Any())
                    return;                            

                var stackPanel = new StackPanel();
                
                foreach(var item in group.Items)
                {
                    var itemValueType = item.Value.GetType();
                    UIElement? uiElement = null;

                    if (TextBoxTypes.Contains(itemValueType))
                    {
                        var displayAttribute = item.Source.SelectAttribute<DisplayAttribute>();

                        var propertyTextBox = new PropertiesTextBox();
                        propertyTextBox.Margin = new Thickness(2);
                        propertyTextBox.LabelControl.Content = displayAttribute?.Name ?? item.Name;                        
                        propertyTextBox.ValidType = itemValueType;
                        propertyTextBox.InitialValue = item.Value.ToString();
                        propertyTextBox.ToolTip = displayAttribute?.Description;
                        propertyTextBox.TextChanged += (object? sender, string e) =>
                        {
                            if (itemValueType == typeof(double) || itemValueType == typeof(float))
                                e = e.Replace(",", ".");

                            item.Source.SetValue(model, Convert.ChangeType(e, itemValueType, new CultureInfo("en-US")));
                        };                        

                        uiElement = propertyTextBox;                        
                    }

                    stackPanel.Children.Add(uiElement);
                }

                var groupBox = new GroupBox
                {
                    Header = group.Name,
                    Content = stackPanel
                };

                PropertiesStack.Children.Add(groupBox);
            }
        }        

        private Group GetOrCreateGroup(string groupName)
        {
            var group = groups.Where(x => x.Name == groupName).FirstOrDefault();

            if (group == null)
            {
                group = new Group(groupName);
                groups.Add(group);
            }    

            return group;            
        }

        private void AddItemInDefaultGroup(GroupItem item)
        {
            var group = GetOrCreateGroup(DefaultGroupName);

            group.Items.Add(item);
        }    
    }
}
