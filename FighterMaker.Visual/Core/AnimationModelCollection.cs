using FighterMaker.Visual.Core.Events;
using FighterMaker.Visual.Models;
using System.Collections;
using System.IO;

namespace FighterMaker.Visual.Core
{
    /// <summary>
    /// Represents a collection of animations.
    /// </summary>
    public class AnimationModelCollection : ICollection<AnimationModel>
    {
        readonly List<AnimationModel> items = [];

        public AnimationModelCollection()
        {
        }        

        public int Count => items.Count;

        public bool IsReadOnly => false;

        /// <summary>
        /// Add a new item to the collection.
        /// </summary>
        /// <param name="animationName"></param>
        /// <exception cref="InvalidOperationException">Throws if an animation with that name already exists.</exception>
        /// <returns>Returns the created animation.</returns>
        public AnimationModel Add(string animationName)
        {
            ThrowIfHasAnimation(animationName);

            var animationModel = new AnimationModel();
            animationModel.BasicValues.NameChanged += AnimationModel_NameChanged;
            animationModel.BasicValues.Name = animationName;            

            items.Add(animationModel);
            return animationModel;
        }

        public void Add(AnimationModel item)
        {            
            ThrowIfHasAnimation(item.BasicValues.Name);

            item.BasicValues.NameChanged += AnimationModel_NameChanged;
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(AnimationModel item)
        {
            return items.Contains(item);
        }

        public void CopyTo(AnimationModel[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<AnimationModel> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public bool Remove(AnimationModel item)
        {
            return items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        void AnimationModel_NameChanged(object? sender, ValuePropertyChangedEventArgs<string> e)
        {
            var any = items.Any(x => x.BasicValues.Name == e.Value);
            e.Accepted = !any;
        }

        void ThrowIfHasAnimation(string animationName)
        {
            var any = items.Any(x => x.BasicValues.Name == animationName);

            if (any)
            {
                throw new InvalidOperationException("There is already an animation with the given name.");
            }
        }
    }
}
