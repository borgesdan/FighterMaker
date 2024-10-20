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
        public class TreeNode
        {
            public TreeNode? RootNode { get; set; } = null;
            public List<PropertyInfoBindingHandler> Properties { get; set; } = [];
        }

        object? _model;

        public PropertiesTreeView()
        {
            InitializeComponent();
        }

        public void SetModel(object model)
        {
            _model = model;
        }

        public void ScanModel()
        {
            InternalScanModel(_model!);
        }

        private void InternalScanModel(object model)
        {
            if (model == null)
                throw new InvalidOperationException();

            var properties = model.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (!IsReadble(property) || !IsBrowsable(property))
                    continue;

                var propertyValue = property.GetValue(model);

                if (propertyValue == null)
                    continue;

                Process(property, propertyValue);
            }
        }

        private void Process(PropertyInfo property, object propertyValue)
        {
            var propertyValueType = propertyValue.GetType();

            if (propertyValueType.IsValueType || propertyValueType == typeof(string))
            {
                ProcessKnowType(property, propertyValue, "");
            }
        }

        private void ProcessKnowType(PropertyInfo property, object propertyValue, string? groupName = null)
        {
            var handler = new PropertyInfoBindingHandler(_model, property);
            MainListView.Items.Add(handler);
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
