using FighterMaker.Visual.Plugins;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FighterMaker.Visual.Models
{
    public class Level2
    {
        public float Level { get; set; }
    }

    public class Level1
    {
        public int Id { get; set; } = Random.Shared.Next();
        public string ActorName { get; set; } = "Teste";
        public Level2 Level2 { get; set; } = new Level2();
    }    

    /// <summary>
    /// Representa um objeto da classe FighterMaker.Mono.Library.Actor
    /// </summary>
    public class ActorModel
    {
        /// <summary>
        /// Obtém ou define o nome do ator.
        /// </summary>        
        [Browsable(false)]
        [Display(Name = "Name", Description = "Obtém ou define o nome do ator")]
        public string? Name { get; set; }

        [Browsable(false)]
        public double PriceTeste { get; set; }

        public Level1 ActorTeste { get; set; } = new Level1();

        /// <summary>
        /// Obtém ou define a lista de plugins (componentes) do ator.
        /// </summary>        
        [Browsable(false)]
        public List<Plugin> Plugins { get; set; } = [];        

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
