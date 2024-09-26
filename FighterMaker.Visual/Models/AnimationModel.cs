using FighterMaker.Visual.Core.Attributes;
using FighterMaker.Visual.Core.Events;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FighterMaker.Visual.Models
{
    public class AnimationModel
    {
        [DisplayName("Basic")]
        [Description("Basics informations.")]
        public AnimationModelBasic BasicValues { get; set; } = new();

        [Ignore]
        public List<AnimationFrameModel> Frames { get; set; } = [];        

        public override string ToString()
        {
            return BasicValues.Name;
        }
    }

    public class AnimationModelBasic
    {
        private string name = string.Empty;
        private double defaultFrameDuration = 0.15;

        /// <summary>
        /// Obtém ou define o nome da animação.
        /// </summary>
        [StringLength(128)]        
        [Description("The name of the animation.")]
        public string Name
        {
            get => name;
            set
            {
                var args = new ValuePropertyChangedEventArgs<string>()
                {
                    Current = name,
                    Value = value
                };

                NameChanged?.Invoke(this, args);
                name = args.Value;
                EndNameChanged?.Invoke(this, name);
            }
        }

        /// <summary>
        /// Obtém ou define padrão de cada quadro da animação.
        /// </summary>
        [DisplayName("Frame Duration")]
        [Description("The default time of each frame of the animation in milliseconds.")]
        public double DefaultFrameDuration
        {
            get => defaultFrameDuration;
            set
            {
                var args = new ValuePropertyChangedEventArgs<double>()
                {
                    Current = defaultFrameDuration,
                    Value = value
                };

                DefaultFrameDurationChanged?.Invoke(this, args);
                defaultFrameDuration = args.Value;
                EndDefaultFrameDurationChanged?.Invoke(this, defaultFrameDuration);
            }
        }

        public event EventHandler<ValuePropertyChangedEventArgs<string>>? NameChanged;
        public event EventHandler<string>? EndNameChanged;
        public event EventHandler<ValuePropertyChangedEventArgs<double>>? DefaultFrameDurationChanged;
        public event EventHandler<double>? EndDefaultFrameDurationChanged;
    }
}
