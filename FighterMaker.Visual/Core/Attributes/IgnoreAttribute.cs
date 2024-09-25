using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Visual.Core.Attributes
{
    public class IgnoreAttribute : Attribute
    {
        public bool Ignore { get; set; } = true;

        public IgnoreAttribute() { }

        public IgnoreAttribute(bool ignore) { Ignore = ignore; }
    }
}
