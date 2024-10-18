using System.Reflection;

namespace FighterMaker.Visual.Core.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static T? GetValue<T>(this PropertyInfo property, object owner)
        {
            var value = property.GetValue(owner);

            if (value != null && value is T)
                return (T)value;

            return default;
        }
    }
}
