using FighterMaker.Visual.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FighterMaker.Visual.Models
{
    public class AnimationFrameModel
    {
        /// <summary>
        /// Obtém ou define o nome do arquivo associado a textura.
        /// </summary>
        public string? BitmapFileName { get; set; }

        /// <summary>
        /// Obtém ou define a texture associada a esse quadro de animação.
        /// </summary>
        public BitmapSource? SourceTexture { get; set; } = null;

        /// <summary>
        /// Obtém ou define os limites do frame.
        /// </summary>
        public Int32Rect Bounds { get; set; }

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
