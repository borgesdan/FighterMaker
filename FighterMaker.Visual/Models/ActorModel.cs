using FighterMaker.Visual.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Visual.Models
{
    public class ActorModel
    {
        public string? Name { get; set; }
        public AnimationModelCollection Animations { get; set; } = [];

        public override string ToString()
        {
            return Name ?? string.Empty;
        }
    }
}
