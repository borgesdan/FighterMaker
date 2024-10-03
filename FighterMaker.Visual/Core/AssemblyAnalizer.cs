using System.Reflection;

namespace FighterMaker.Visual.Core
{
    public class AssemblyAnalizer
    {
        private Type[]? typeBuffer = null;

        public Assembly CurrentAssembly { get; set; }

        public AssemblyAnalizer(Assembly assembly) 
        {
            this.CurrentAssembly = assembly;
        }

        public List<Type> GetTypesWithAttribute<T>() where T : Attribute
        {
            typeBuffer ??= CurrentAssembly.GetTypes();            

            List<Type> result = new List<Type>();

            foreach (var type in typeBuffer)
            {
                var attributes = type.GetCustomAttributes();

                if (attributes.Any(a => a is T))
                {
                    result.Add(type);
                }
            }

            return result;
        }

        public static AssemblyAnalizer FromExecutingAssembly()
        {
            return new AssemblyAnalizer(Assembly.GetExecutingAssembly());
        }
    }
}
