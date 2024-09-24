using System.Text;

namespace FighterMaker.Visual.Core.Events
{
    public class StringPropertyChangedEventArgs : EventArgs
    {
        public string? Current { get; set; } = null;
        public StringBuilder? Value { get; set; } = null;
    }
}
