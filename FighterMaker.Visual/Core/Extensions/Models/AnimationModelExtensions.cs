using FighterMaker.Visual.Models;

namespace FighterMaker.Visual.Core.Extensions.Models
{
    public static class AnimationModelExtensions
    {
        public static AnimationModel WithName(this AnimationModel model, string name)
        {
            model.BasicValues.Name = name;
            return model;
        }
    }
}
