using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FighterMaker.Mono.Library
{
    /// <summary>
    /// Representa um quadro de uma animação.
    /// </summary>
    public class AnimationFrame
    {
        /// <summary>
        /// Obtém ou define a texture associada a esse quadro de animação.
        /// </summary>
        public Texture2D SourceTexture { get; set; } = null;

        /// <summary>
        /// Obtém ou define os limites do frame.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Obtém ou define a duração de exibição do frame em milisegundos.
        /// Caso nulo a classe de animação que recebe esse quadro deve definir esse valor.
        /// </summary>
        public int? Duration { get; set; } = null;

        /// <summary>
        /// Obtém ou define os efeitos de espelhamento do frame.
        /// </summary>
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;

        /// <summary>
        /// Obtém ou define a posição a posição do eixo de exibição do quadro.
        /// Caso nulo a classe de animação que recebe esse quadro deve definir esse valor.
        /// </summary>
        public Vector2? Origin { get; set; } = null;

        /// <summary>
        /// Obtém ou define a escala de tamanho do frame
        /// Caso nulo a classe de animação que recebe esse quadro deve definir esse valor.
        /// </summary>
        public Vector2? Scale { get; set; } = null;
    }
}
