using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Visual.Core.Attributes
{
    public class ActorComponentAttribute : Attribute
    {
        public string? Name { get; set; }
        public bool Enabled { get; set; }
    }
}
