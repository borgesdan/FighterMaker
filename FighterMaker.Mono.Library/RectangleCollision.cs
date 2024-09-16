using Microsoft.Xna.Framework;

namespace FighterMaker.Mono.Library
{
    /// <summary>
    /// Define um retângulo que contém propriedades para checagem de colisão.
    /// </summary>
    public class RectangleCollision
    {
        float damage = 0;

        /// <summary>
        /// Obtém ou define os limites do retângulo.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Obtém ou define se a colisão está habilitada.
        /// </summary>
        public bool EnabledCollision { get; set; }

        /// <summary>
        /// Obtém ou define a porcentagem de dano a ser recebida em colisão, de 0 a 1F.
        /// </summary>
        public float Damage { get => damage; set => damage = MathHelper.Clamp(value, 0, 1); }
    }
}
