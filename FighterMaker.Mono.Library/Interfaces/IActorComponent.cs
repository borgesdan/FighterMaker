using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FighterMaker.Mono.Library.Interfaces
{
    public interface IActorComponent
    {
        bool IsEnabled { get; set; }
        bool IsVisible { get; set; }
        void Attach(Actor actor);

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
