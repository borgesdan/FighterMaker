using FighterMaker.Visual.Controls.PropertiesControlChildren;
using FighterMaker.Visual.Core;
using FighterMaker.Visual.Core.Extensions;
using System;
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
    /// Interaction logic for PropertiesTreeView.xaml
    /// </summary>
    public partial class PropertiesTreeView : UserControl
    {
        public class PropertyNode(object model)
        {
            public object Model { get; set; } = model;
            public List<PropertyNode> PropertyNodes { get; set; } = [];
            public List<PropertyInfoBindingHandler> Properties { get; set; } = [];
        }

        object? _model;
        List<PropertyNode> propertyNodes = [];        

        public PropertiesTreeView()
        {
            InitializeComponent();
        }

        public void SetModel(object model)
        {
            _model = model;
            propertyNodes.Add(new PropertyNode(_model));
        }

        public void ScanModel()
        {
            InternalScanModel(propertyNodes.First());
            PopulateControl();
        }

        private void InternalScanModel(PropertyNode propertyNode)
        {
            var model = propertyNode.Model ?? throw new InvalidOperationException();
            var properties = model.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (!IsReadble(property) || !IsBrowsable(property))
                    continue;

                var propertyValue = property.GetValue(model);

                if (propertyValue == null)
                    continue;

                Process(property, propertyValue, propertyNode);
            }
        }

        private void Process(PropertyInfo property, object propertyValue, PropertyNode propertyNode)
        {
            var propertyValueType = propertyValue.GetType();

            if (propertyValueType.IsPrimitive || propertyValueType == typeof(string))
            {
                ProcessKnowType(property, propertyNode);
            } 
            else if (propertyValueType.IsClass)
            {
                ProcessReferenceType(property, propertyNode);
            }
        }

        private void ProcessKnowType(PropertyInfo property, PropertyNode propertyNode)
        {
            //var handler = new PropertyInfoBindingHandler(_model, property);
            //MainListView.Items.Add(handler);

            var propertyBinding = new PropertyInfoBindingHandler(propertyNode.Model, property);
            propertyNode.Properties.Add(propertyBinding);
        }

        private void ProcessReferenceType(PropertyInfo property, PropertyNode propertyNode)
        {
            var value = property.GetValue(propertyNode.Model);
            var node = new PropertyNode(value);
            propertyNode.PropertyNodes.Add(node);

            InternalScanModel(node);
        }

        private void PopulateControl()
        {
            foreach (var node in propertyNodes)
            {
                var treeViewItem = PopulateNode(node, null);                
            }
        }   
        
        private TreeViewItem PopulateNode(PropertyNode node, TreeViewItem? rootNode)
        {
            var item = new TreeViewItem();
            item.Header = node.Model.ToString();
            item.Tag = node.Model;
            item.IsExpanded = true;

            if (node.Properties.Any())
            {
                var listView = new ListView();
                var gridView = new GridView();

                gridView.Columns.Add(new GridViewColumn()
                {
                    Header = "Name",
                    DisplayMemberBinding = new Binding("Name")
                });

                gridView.Columns.Add(new GridViewColumn()
                {
                    Header = "Value",
                    DisplayMemberBinding = new Binding("Value")
                });

                listView.View = gridView;
                listView.ItemsSource = node.Properties;

                item.Items.Add(listView);
            }            

            if (rootNode == null)
                MainTreeView.Items.Add(item);
            else
                rootNode.Items.Add(item);

            if (node.PropertyNodes.Any())
            {
                foreach (var propertyNode in node.PropertyNodes)
                {
                    PopulateNode(propertyNode, item);
                }
            }

            return item;
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
