using FighterMaker.Mono.Library.Enumerations;
using FighterMaker.Mono.Library.Extensions;
using FighterMaker.Mono.Library.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FighterMaker.Mono.Library
{
    /// <summary>
    /// Representa uma animação baseada em uma folha de sprites.
    /// </summary>
    public class SpriteSheetAnimation : IAnimation
    {
        static readonly Vector2 DefaultScale = Vector2.One;

        AnimationFrame currentFrame;
        int currentFrameIndex;
        int elapsedTime = 0;

        /// <summary>
        /// Obtém ou define a textura que representa a folha de sprites.
        /// </summary>
        public Texture2D SpriteSheetTexture { get; set; } = null;

        /// <summary>
        /// Obtém ou define o nome da animação.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Obtém ou define os frames da animação contidos na folha de sprites.
        /// </summary>
        public List<AnimationFrame> Frames { get; set; }

        /// <summary>
        /// Obtém ou define a posição do desenho do frame na janela de jogo.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Obtém ou define a duração padrão de cada frame em milisegundos.
        /// </summary>
        public int Duration { get; set; } = 15;

        /// <summary>
        /// Obtém ou define o ponto de origem padrão para exibição de cada quadro.
        /// </summary>
        public OriginPoint DefaultOrigin { get; set; } = OriginPoint.TopLeft;

        public SpriteSheetAnimation(string animationName, List<AnimationFrame> frames)
        {
            Name = animationName;
            Frames = frames ?? new List<AnimationFrame>();
        }

        public SpriteSheetAnimation(string animationName, List<AnimationFrame> frames, Texture2D spriteSheetTexture) : this(animationName, frames)
        {
            SpriteSheetTexture = spriteSheetTexture ?? throw new ArgumentNullException(nameof(spriteSheetTexture));
            Frames.ForEach(f => f.SourceTexture = spriteSheetTexture);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (currentFrame == null)
                return;

            var sourceRectangle = currentFrame.Bounds;
            var spriteEffects = currentFrame.Effects;
            var origin = currentFrame.Origin ?? DefaultOrigin.ToVector2(sourceRectangle);
            var scale = currentFrame.Scale ?? DefaultScale;

            spriteBatch.Draw(SpriteSheetTexture, Position, sourceRectangle, Color.White, 0, origin, scale, spriteEffects, 0);
        }

        public void Update(GameTime gameTime)
        {
            SetCurrentFrame(gameTime);
        }

        private void SetCurrentFrame(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            var currentFrameDuration = currentFrame?.Duration;

            if ((currentFrameDuration != null && currentFrame.Duration < elapsedTime) || Duration < elapsedTime)
            {
                ++currentFrameIndex;                

                if (currentFrameIndex >= Frames.Count)
                    currentFrameIndex = 0;
                
                elapsedTime = 0;
            }

            currentFrame = Frames[currentFrameIndex];
        }
    }
}
