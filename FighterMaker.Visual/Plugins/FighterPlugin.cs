using FighterMaker.Visual.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Visual.Plugins
{
    [Plugin(Name = "Fighter")]
    public class FighterPlugin : Plugin
    {
        [DisplayName("Max Life")]
        public int MaxLife { get; set; }
        [DisplayName("Max Special Bar")]
        public int MaxSpecialBar { get; set; }
        [DisplayName("Special Bar Count")]
        public int SpecialBarCount { get; set; }

        public AnimationPlugin AnimationPlugin { get; set; } = new AnimationPlugin();

        public FighterPlugin() 
        {
        }
    }
}
