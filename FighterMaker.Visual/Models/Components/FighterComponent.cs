using FighterMaker.Visual.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Visual.Models.Components
{
    [ActorComponent(Name = "Fighter")]
    public class FighterComponent : ActorComponentModel
    {
        public FighterComponentPage Page { get; set; }

        public FighterComponent(ActorModel actor) : base(actor)
        {
        }
    }
}
