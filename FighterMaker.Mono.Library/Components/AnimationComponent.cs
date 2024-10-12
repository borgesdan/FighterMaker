using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FighterMaker.Mono.Library.Components
{
    public class AnimationComponent : ActorComponent
    {
        public List<SpriteSheetAnimation> Animations { get; set; } = new List<SpriteSheetAnimation>();
        public SpriteSheetAnimation Current { get; private set; } = null;

        public void SetCurrent(int index)
        {
            if (index < 0 || index > Animations.Count - 1)
                return;

            Current = Animations[index];
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            Current.Position = Actor.Transform.Position2;
            Current?.Update(gameTime);

            base.OnUpdate(gameTime);
        }

        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Current?.Draw(gameTime, spriteBatch);

            base.OnDraw(gameTime, spriteBatch);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }
    }
}
