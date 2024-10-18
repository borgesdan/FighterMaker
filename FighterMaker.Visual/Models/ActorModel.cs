using FighterMaker.Visual.Plugins;
using System.ComponentModel.DataAnnotations;

namespace FighterMaker.Visual.Models
{
    [Display(Name = "Teste")]
    public class TesteClass
    {
        public int Id { get; set; }
    }

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

        [Display(GroupName = "Teste Group")]
        public TesteClass Teste { get; set; } = new TesteClass();

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
