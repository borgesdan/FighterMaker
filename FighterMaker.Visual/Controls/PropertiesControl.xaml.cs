using FighterMaker.Visual.Controls.PropertiesControlChildren;
using FighterMaker.Visual.Controls.PropertiesViewChildren;
using FighterMaker.Visual.Core.Attributes;
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
    /// Representa um controle para exibir campos com valores das propriedades de um modelo.
    /// </summary>
    public partial class PropertiesControl : UserControl
    {
        readonly StackPanel mainStackPanel = new StackPanel();

        private class GroupItem
        {
            public PropertyInfo Source { get; set; } = null!;
            public string Name { get; set; } = string.Empty;
            public object Value { get; set; } = null!;
            public bool IsItemInCollection { get; set; }
            public IList Collection { get; set; } = null!;
            public int ItemInCollectionIndex { get; set; } = 0;
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
            InternalAnalizeModel(model);
        }        

        private void InternalAnalizeModel(object internalModel, string? groupName = null, bool createPageForReferenceType = false)
        {
            if (internalModel == null)
                throw new InvalidOperationException();

            var properties = internalModel.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (!IsReadble(property) || !IsBrowsable(property))
                    continue;

                var propValue = property.GetValue(internalModel);

                if (propValue == null)
                    continue;                                    

                if (string.IsNullOrWhiteSpace(groupName))
                {
                    groupName = GetGroupName(property, internalModel);
                }

                Process(property, propValue, groupName);                
            }
        }   

        public void PopulateControl()
        {
            foreach (var group in groups)
            {
                if (!group.Items.Any())
                    return;

                var stackPanel = new StackPanel();

                foreach (var item in group.Items)
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

                            var value = Convert.ChangeType(e, itemValueType, new CultureInfo("en-US"));

                            if (item.IsItemInCollection)
                            {
                                item.Collection[item.ItemInCollectionIndex] = value;
                            }
                            else
                            {
                                item.Source.SetValue(model, value);
                            }
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

                mainStackPanel.Children.Add(groupBox);
            }

            Page page = new Page();
            page.Content = mainStackPanel;
            MainFrame.Content = page;
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

        private void AddItemInGroup(GroupItem item, string groupName)
        {
            var group = GetOrCreateGroup(groupName);
            group.Items.Add(item);
        }

        private void AddItemInDefaultGroup(GroupItem item)
        {
            AddItemInGroup(item, DefaultGroupName);
        }

        private void Process(PropertyInfo property, object propertyValue, string? groupName, bool createPageForReferenceType = false)
        {
            var propValueType = propertyValue.GetType();

            if (propValueType.IsValueType || propValueType == typeof(string))
            {
                ProcessKnowType(property, propertyValue, groupName);
            }
            else if (propValueType.IsArray || propValueType.IsGenericList())
            {
                ProcessCollection(property, propertyValue, groupName);
            }
            else if (propValueType.IsClass)
            {
                if (createPageForReferenceType)
                {
                    return;
                }

                ProcessReferenceType(property, propertyValue);
            }
        }

        private void ProcessKnowType(PropertyInfo property, object propertyValue, string? groupName = null)
        {
            var groupItem = new GroupItem
            {
                Source = property,
                Name = property.SelectAttribute<DisplayAttribute>()?.GroupName ?? property.Name,
                Value = propertyValue
            };

            var _groupName = groupName ?? DefaultGroupName;
            AddItemInGroup(groupItem, _groupName);
        }

        private void ProcessCollection(PropertyInfo property, object propertyValue, string? groupName = null)
        {
            if (propertyValue is not IList collection)
                throw new InvalidOperationException();

            var index = 0;
            foreach (var listItem in collection)
            {
                if (!listItem.GetType().IsValueType)
                {
                    ProcessReferenceType(property, listItem);
                    continue;
                }

                var item = new GroupItem
                {
                    Source = property,
                    Name = property.SelectAttribute<DisplayAttribute>()?.GroupName ?? property.Name,
                    Value = listItem,
                    IsItemInCollection = true,
                    Collection = collection,
                    ItemInCollectionIndex = index
                };

                var _groupName = groupName ?? DefaultGroupName;
                AddItemInGroup(item, _groupName);

                index++;
            }
        }

        private void ProcessReferenceType(PropertyInfo property, object propertyValue)
        {
            var displayAttribute = property.SelectAttribute<DisplayAttribute>();
            InternalAnalizeModel(propertyValue, displayAttribute?.GroupName, true);
        }

        private string? GetGroupName(PropertyInfo property, object internalModel)
        {
            string? groupName = null;
            var displayAttribute = property.SelectAttribute<DisplayAttribute>();

            if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.GroupName))
            {
                groupName = displayAttribute.GroupName;
            }
            else if (internalModel == model)
            {
                groupName = DefaultGroupName;
            }
            else
            {
                var modelDisplayAttribute = internalModel.GetAttribute<DisplayAttribute>();
                groupName = modelDisplayAttribute?.GroupName ?? modelDisplayAttribute?.Name;
            }

            return groupName;
        }


        private static bool IsReadble(PropertyInfo property)
        {
            return property is null || property.CanRead;
        }

        private static bool IsBrowsable(PropertyInfo property)
        {
            var browsableAttribute = property.SelectAttribute<BrowsableAttribute>()?.Browsable;
            return browsableAttribute ?? true;
        }
    }
}
