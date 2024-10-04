using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Mono.Library
{
    /// <summary>
    /// Fornece acesso as manipulações de transformação (posição, escala e rotação) de um objeto.
    /// </summary>
    public class Transform : IEquatable<Transform>
    {

        private Vector3 _oldPosition = Vector3.Zero;
        private Vector3 _oldScale = Vector3.One;
        private Vector3 _oldRotation = Vector3.Zero;

        private Vector3 _position = Vector3.Zero;
        private Vector3 _scale = Vector3.One;
        private Vector3 _rotation = Vector3.Zero;

        /// <summary>Obtém ou define a posição nos eixos X, Y e Z.</summary>
        public Vector3 Position3
        {
            get => _position;
            set
            {
                _oldPosition = _position;
                _position = value;
            }
        }

        /// <summary>Obtém ou define a posição no eixo X.</summary>
        public float X { get { return Position3.X; } set { Position3 = new Vector3(value, Y, Z); } }
        /// <summary>Obtém ou define a posição no eixo Y.</summary>
        public float Y { get { return Position3.Y; } set { Position3 = new Vector3(X, value, Z); } }
        /// <summary>Obtém ou define a posição no eixo Z.</summary>
        public float Z { get { return Position3.Z; } set { Position3 = new Vector3(X, Y, value); } }
        /// <summary>Obtém ou define a posição através de um Vector2 ignorando o eixo Z.</summary>
        public Vector2 Position2 { get => new Vector2(Position3.X, Position3.Y); set => Position3 = new Vector3(value.X, value.Y, Position3.Z); }

        /// <summary>Obtém ou define a escala nos eixos X, Y e Z.</summary>
        public Vector3 Scale3
        {
            get => _scale;
            set
            {
                _oldScale = _scale;
                _scale = value;
            }
        }
        /// <summary>Obtém ou define a escala no eixo X.</summary>
        public float Xs { get { return Scale3.X; } set { Scale3 = new Vector3(value, Ys, Zs); } }
        /// <summary>Obtém ou define a escala no eixo Y.</summary>
        public float Ys { get { return Scale3.Y; } set { Scale3 = new Vector3(Xs, value, Zs); } }
        /// <summary>Obtém ou define a escala no eixo Z.</summary>
        public float Zs { get { return Scale3.Z; } set { Scale3 = new Vector3(Xs, Ys, value); } }
        /// <summary>Obtém ou define a escala através de um Vector2 ignorando o eixo Z.</summary>
        public Vector2 Scale2 { get => new Vector2(Scale3.X, Scale3.Y); set => Scale3 = new Vector3(value.X, value.Y, Scale3.Z); }

        /// <summary>Obtém ou define a rotação nos eixos X, Y e Z.</summary>
        public Vector3 Rotation3
        {
            get => _rotation;
            set
            {
                _oldRotation = _rotation;
                _rotation = value;
            }
        }
        /// <summary>Obtém ou define a rotação no eixo X.</summary>
        public float Xr { get { return Rotation3.X; } set { Rotation3 = new Vector3(value, Yr, Zr); } }
        /// <summary>Obtém ou define a rotação no eixo Y.</summary>
        public float Yr { get { return Rotation3.Y; } set { Rotation3 = new Vector3(Xr, value, Zr); } }
        /// <summary>Obtém ou define a rotação no eixo Z.</summary>
        public float Zr { get { return Rotation3.Z; } set { Rotation3 = new Vector3(Xr, Yr, value); } }
        /// <summary>Obtém ou define a rotação em um plano 2D (obtém ou define o valor de Rz)</summary>
        public float Rotation2 { get => Zr; set => Zr = value; }        

        /// <summary>
        /// Inicializa uma nova instância da classe.
        /// </summary>
        public Transform() { }

        /// <summary>
        /// Inicializa uma nova instância da classe.
        /// </summary>
        /// <param name="position">Informa o valor da posição.</param>
        /// <param name="scale">Informa o valor da escala.</param>
        /// <param name="rotation">Informa o valor da rotação. Para um jogo 2D somente o valor no eixo Z será considerado.</param>
        public Transform(Vector3 position, Vector3 scale, Vector3 rotation)
        {
            this.Position3 = position;
            this.Scale3 = scale;
            this.Rotation3 = rotation;
        }

        /// <summary>
        /// Inicializa uma nova instância da classe.
        /// </summary>
        /// <param name="position">Informa o valor da posição.</param>
        /// <param name="scale">Informa o valor da escala.</param>
        /// <param name="rotation">Informa o valor da rotação.</param>
        public Transform(Vector2 position, Vector2 scale, float rotation)
            : this(new Vector3(position, 0), new Vector3(scale, 1), new Vector3(0, 0, rotation))
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe como cópia de outra instância
        /// </summary>
        /// <param name="source">A instância a ser copiada</param>
        public Transform(Transform source)
        {
            this.Position3 = source.Position3;
            this.Scale3 = source.Scale3;
            this.Rotation3 = source.Rotation3;
        }

        /// <summary>
        /// Copia as propriedades de um objeto Transform.
        /// </summary>
        /// <param name="transform"></param>
        public void Set(Transform transform)
        {
            this.Position3 = transform.Position3;
            this.Scale3 = transform.Scale3;
            this.Rotation3 = transform.Rotation3;
        }        

        ///<inheritdoc/>
        public override string ToString()
        {
            return string.Concat("Position: ", Position3, " ", "Scale: ", Scale3, " ", "Rotation: ", Rotation3);
        }

        ///<inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals(obj as Transform);
        }

        ///<inheritdoc/>
        public bool Equals(Transform other)
        {
            return other != null &&
                   Position3.Equals(other.Position3) &&
                   Scale3.Equals(other.Scale3) &&
                   Rotation3.Equals(other.Rotation3);
        }

        ///<inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Position3, Scale3, Rotation3);
        }

        ///<inheritdoc/>
        public static bool operator ==(Transform left, Transform right)
        {
            return EqualityComparer<Transform>.Default.Equals(left, right);
        }

        ///<inheritdoc/>
        public static bool operator !=(Transform left, Transform right)
        {
            return !(left == right);
        }
    }
}
