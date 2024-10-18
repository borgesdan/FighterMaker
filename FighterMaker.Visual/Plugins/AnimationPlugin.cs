using FighterMaker.Visual.Core.Attributes;
using FighterMaker.Visual.Models;
using FighterMaker.Visual.Plugins.Animation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FighterMaker.Visual.Plugins
{
    [Display(Name = "Animation")]
    [Plugin(Name = "Animation")]
    public class AnimationPlugin : Plugin
    {
        public int Teste { get; set; }

        [Browsable(false)]
        public PluginPage Page { get; set; } = new();

        public AnimationPlugin() 
        {            
        }

        protected override void OnAttach(ActorModel model)
        {
            Page.SetModel(model);
            base.OnAttach(model);
        }        
    }
}
