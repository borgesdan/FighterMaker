using FighterMaker.Mono.Library.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FighterMaker.Mono.Library
{
    public class FighterActor : IActor
    {
        public List<SpriteSheetAnimation> Animations { get; set; } = new List<SpriteSheetAnimation>();
        
        public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
