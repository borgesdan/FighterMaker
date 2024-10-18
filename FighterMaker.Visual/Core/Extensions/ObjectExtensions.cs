using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Visual.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static T? GetAttribute<T>(this object obj) where T : Attribute
        { 
            var attributes = obj.GetType().GetCustomAttributes<T>();            
            var attr = attributes.FirstOrDefault();

            return attr;
        }
    }
}
