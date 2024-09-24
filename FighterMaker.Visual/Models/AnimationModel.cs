using FighterMaker.Visual.Core.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Visual.Models
{
    public class AnimationModel
    {
        private string name;

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

        public event EventHandler<StringPropertyChangedEventArgs> NameChanged;

        public override string ToString()
        {
            return Name;
        }
    }

    
}
