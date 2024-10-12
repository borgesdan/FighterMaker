namespace FighterMaker.Visual.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsGenericList(this Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>));
        }
    }
}
