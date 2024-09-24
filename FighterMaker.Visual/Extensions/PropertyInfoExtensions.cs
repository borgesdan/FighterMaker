using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Visual.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string GetValueAsString(this PropertyInfo property, object owner)
        {
            var value = property.GetValue(owner);

            if (value != null && value is string)
                return (string)value;

            return string.Empty;
        }
    }
}
