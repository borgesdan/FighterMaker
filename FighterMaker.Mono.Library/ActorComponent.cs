using FighterMaker.Mono.Library.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FighterMaker.Mono.Library
{
    public class ActorComponent : IActorComponent
    {
        public bool IsEnabled { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        public Actor Actor { get; protected set; }

        public ActorComponent() { }

        public void Attach(Actor actor)
        {
            Actor = actor;
        }

        public void Update(GameTime gameTime)
        {
            if (IsEnabled)
            {
                OnUpdate(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                OnDraw(gameTime, spriteBatch);
            }
        }

        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}
