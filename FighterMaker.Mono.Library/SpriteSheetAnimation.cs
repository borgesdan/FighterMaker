using FighterMaker.Mono.Library.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FighterMaker.Mono.Library
{
    /// <summary>
    /// Representa um frame de uma textura de folha de sprites.
    /// </summary>
    public class SpriteSheetFrame
    {
        /// <summary>
        /// Obtém ou define os limites do frame.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Obtém ou define a duração do frame em milisegundos.
        /// Caso nulo será utilizado o valor da propriedade Duration da classe SpriteSheetAnimation.
        /// </summary>
        public int? Duration { get; set; } = null;

        /// <summary>
        /// Obtém ou define os efeitos de espelhamento do frame.
        /// </summary>
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        /// <summary>
        /// Obtém ou define a posição do eixo do frmae.
        /// </summary>
        public Vector2? Origin { get; set; } = null;

        /// <summary>
        /// Obtém ou define a escala de tamanho do frame
        /// </summary>
        public Vector2? Scale { get; set; } = null;
    }

    /// <summary>
    /// Representa uma animação baseada em uma folha de sprites.
    /// </summary>
    public class SpriteSheetAnimation : IAnimation
    {
        static readonly Vector2 DefaultOrigin = Vector2.Zero;
        static readonly Vector2 DefaultScale = Vector2.One;

        SpriteSheetFrame currentFrame;
        int currentFrameIndex;
        int elapsedTime = 0;

        /// <summary>
        /// Obtém ou define a textura que representa a folha de sprites.
        /// </summary>
        public Texture2D SpriteSheetTexture { get; set; }

        /// <summary>
        /// Obtém ou define o nome da animação.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Obtém ou define os frames da animação contidos na folha de sprites.
        /// </summary>
        public List<SpriteSheetFrame> Frames { get; set; }

        /// <summary>
        /// Obtém ou define a posição do desenho do frame na janela de jogo.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Obtém ou define a duração padrão de cada frame em milisegundos.
        /// </summary>
        public int Duration { get; set; } = 15;           

        public SpriteSheetAnimation(string Name, List<SpriteSheetFrame> frames)
        {
            this.Name = Name;
            this.Frames = frames ?? new List<SpriteSheetFrame>();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (currentFrame == null)
                return;

            var sourceRectangle = currentFrame.Bounds;
            var spriteEffects = currentFrame.Effects;
            var origin = currentFrame.Origin ?? DefaultOrigin;
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
