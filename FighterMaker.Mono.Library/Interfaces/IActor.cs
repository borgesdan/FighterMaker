﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FighterMaker.Mono.Library.Interfaces
{
    public interface IActor
    {
        Transform Transform { get; set; }
        bool IsEnabled { get; set; }
        bool IsVisible { get; set; }
        List<ActorComponent> Components { get; set; }

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
