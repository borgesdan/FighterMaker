using FighterMaker.Visual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Visual.Plugins
{
    public class Plugin
    {
        public ActorModel? Model { get; protected set; } = null;

        public void Attach(ActorModel model)
        {
            Model = model;
            OnAttach(model);
        }

        protected virtual void OnAttach(ActorModel model)
        {

        }
    }
}
