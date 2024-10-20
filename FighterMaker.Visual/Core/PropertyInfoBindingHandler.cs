using FighterMaker.Visual.Core.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FighterMaker.Visual.Core
{
    public class PropertyInfoBindingHandler
    {
        static readonly Dictionary<Type, Func<string, bool>> KnownTypesParseFuncs = new Dictionary<Type, Func<string, bool>>
        {
            { typeof(char), (s) => char.TryParse(s, out _) },
            { typeof(byte), (s) => byte.TryParse(s, out _) },
            { typeof(sbyte), (s) => sbyte.TryParse(s, out _) },
            { typeof(short), (s) => short.TryParse(s, out _) },
            { typeof(ushort), (s) => ushort.TryParse(s, out _) },
            { typeof(int), (s) => int.TryParse(s, out _) },
            { typeof(uint), (s) => uint.TryParse(s, out _) },
            { typeof(long), (s) => long.TryParse(s, out _) },
            { typeof(ulong), (s) => ulong.TryParse(s, out _) },
            { typeof(double), (s) => double.TryParse(s, out _) },
            { typeof(float), (s) => float.TryParse(s, out _) },
            { typeof(decimal), (s) => decimal.TryParse(s, out _) }
        };

        DisplayAttribute? displayAttribute = null;
        object model;

        public PropertyInfo Property { get; set; }

        public string Name
        {
            get => displayAttribute?.Name ?? Property.Name;
            set
            {
                if (displayAttribute != null)
                    displayAttribute.Name = value;
            }
        }

        public object? Value
        {
            get => Property.GetValue(model);
            set
            {
                var propertyType = Property.PropertyType;                

                if (KnownTypesParseFuncs.TryGetValue(propertyType, out var resultType))
                {
                    if (resultType.Invoke(value.ToString()))
                    {
                        var changedValue = Convert.ChangeType(value, propertyType);
                        Property.SetValue(model, changedValue);
                    }
                }               
            }
        }

        public PropertyInfoBindingHandler(object propertyOwner, PropertyInfo propertyInfo)
        {
            model = propertyOwner;
            Property = propertyInfo;
            displayAttribute = Property.SelectAttribute<DisplayAttribute>();
        }
    }
}
