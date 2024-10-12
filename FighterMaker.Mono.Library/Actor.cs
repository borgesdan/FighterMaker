using FighterMaker.Mono.Library.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FighterMaker.Mono.Library
{
    public sealed class Actor : IActor
    {
        public Transform Transform { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        public List<IActorComponent> Components { get; set; } = new List<IActorComponent>();

        public void Initialize() 
        {            
        }

        public void AddComponent(IActorComponent component)
        {            
            component.Attach(this);
            component.Initialize();
            Components.Add(component);
        }
        
        public void LoadContent() 
        {
        }

        public void Update(GameTime gameTime) 
        {
            if (IsEnabled)
            {
                Components.ForEach(x => x.Update(gameTime));
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) 
        {
            if (IsVisible)
            {
                Components.ForEach(x => x.Draw(gameTime, spriteBatch));
            }
        }        
    }
}
