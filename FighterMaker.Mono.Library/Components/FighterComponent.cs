using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Mono.Library.Components
{
    public class FighterComponent : ActorComponent
    {
        AnimationComponent animationComponent;

        protected override void OnInitialize()
        {
            animationComponent = AddComponent<AnimationComponent>();

            base.OnInitialize();
        }
    }
}
