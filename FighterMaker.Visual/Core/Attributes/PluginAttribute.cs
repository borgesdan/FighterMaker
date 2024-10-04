namespace FighterMaker.Visual.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginAttribute : Attribute
    {        
        public string? Name { get; set; }
    }
}
