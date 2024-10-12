using FighterMaker.Visual.Core.Attributes;
using FighterMaker.Visual.Models;
using FighterMaker.Visual.Plugins.Animation;

namespace FighterMaker.Visual.Plugins
{
    [Plugin(Name = "Animation")]
    public class AnimationPlugin : Plugin
    {
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
