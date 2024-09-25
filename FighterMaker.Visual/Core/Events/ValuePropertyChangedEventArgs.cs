using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Visual.Core.Events
{
    public class ValuePropertyChangedEventArgs<T> : EventArgs
    {
        public T? Current { get; set; }
        public T? Value { get; set; }
    }
}
