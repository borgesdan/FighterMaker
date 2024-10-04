﻿using FighterMaker.Mono.Library.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FighterMaker.Mono.Library
{
    public abstract class Actor : IActor
    {
        public Transform Transform { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        public List<ActorComponent> Components { get; set; } = new List<ActorComponent>();

        public void Update(GameTime gameTime) 
        {
            if (IsEnabled)
            {
                OnUpdate(gameTime);
                Components.ForEach(x => x.Update(gameTime));
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) 
        {
            if (IsVisible)
            {
                OnDraw(gameTime, spriteBatch);
                Components.ForEach(x => x.Draw(gameTime, spriteBatch));
            }
        }

        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}
