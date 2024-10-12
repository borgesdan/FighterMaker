using FighterMaker.Visual.Plugins;
using System.ComponentModel.DataAnnotations;

namespace FighterMaker.Visual.Models
{
    /// <summary>
    /// Representa um objeto da classe FighterMaker.Mono.Library.Actor
    /// </summary>
    public class ActorModel
    {
        /// <summary>
        /// Obtém ou define o nome do ator.
        /// </summary>
        [Display(Name = "Name", Description = "Obtém ou define o nome do ator")]
        public string? Name { get; set; }

        /// <summary>
        /// Obtém ou define a lista de plugins (componentes) do ator.
        /// </summary>        
        public List<Plugin> Plugins { get; set; } = [];

        public List<double> Teste { get; set; } = new List<double>
        {
            5.6,
            6.7,
            8.8
        };

        public void AddPlugin(Plugin plugin)
        {
            plugin.Attach(this);
            Plugins.Add(plugin);
        }

        public override string ToString()
        {
            return Name ?? string.Empty;
        }
    }

    public static class ActorModelExtensions
    {
        public static ActorModel WithName(this ActorModel model, string name)
        {
            model.Name = name;
            return model;
        }
    }
}
