using FighterMaker.Mono.Library.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;

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

        public virtual void Initialize() 
        {
            OnInitialize();
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
        protected virtual void OnInitialize() { }

        protected T AddComponent<T>() where T : ActorComponent, new()
        {
            var component = new T();
            Actor.AddComponent(component);

            return component;
        }

        protected T GetOrAddComponent<T>() where T : ActorComponent, new()
        {
            var component = Actor.Components
                .Where(x => x.GetType() == typeof(T))
                .FirstOrDefault() as T ?? new T();

            return component;
        }
    }
}
