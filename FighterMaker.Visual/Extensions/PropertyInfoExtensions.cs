using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Visual.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static T? GetValue<T>(this PropertyInfo property, object owner)
        {
            var value = property.GetValue(owner);

            if(value != null && value is T) 
                return (T)value;

            return default(T);
        }

        public static T? SelectAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            var attributes = property.GetCustomAttributes(typeof(T), false);
            var attr = attributes.FirstOrDefault();

            return attr as T;
        }
    }
}
