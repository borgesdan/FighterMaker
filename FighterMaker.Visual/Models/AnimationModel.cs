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

        public override string ToString()
        {
            return BasicValues.Name;
        }
    }
    
    public class AnimationModelBasic
    {
        private string name = string.Empty;

        [Browsable(true)]
        [StringLength(128)]
        [DisplayName("Name")]
        [Description("The name of the animation.")]
        public string Name
        {
            get => name;
            set
            {
                var args = new StringPropertyChangedEventArgs()
                {
                    Current = name,
                    Value = new StringBuilder(value)
                };

                NameChanged?.Invoke(this, args);

                name = args.Value.ToString();
            }
        }

        public event EventHandler<StringPropertyChangedEventArgs>? NameChanged;
    }
}
