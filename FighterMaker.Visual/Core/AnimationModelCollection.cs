using FighterMaker.Visual.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FighterMaker.Visual.Core
{
    public class AnimationModelCollection : ICollection<AnimationModel>
    {
        List<AnimationModel> items = [];

        public AnimationModelCollection()
        {
        }

        void AnimationModel_NameChanged(object? sender, Core.Events.ValuePropertyChangedEventArgs<string> e)
        {
            var any = items.Any(x => x.BasicValues.Name == e.Value);
            e.Accepted = !any;
        }        

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public AnimationModel Add(string animationName)
        {
            var animationModel = new AnimationModel();
            animationModel.BasicValues.NameChanged += AnimationModel_NameChanged;
            animationModel.BasicValues.Name = animationName;            

            items.Add(animationModel);
            return animationModel;
        }

        public void Add(AnimationModel item)
        {
            var any = items.Any(x => x.BasicValues.Name == item.BasicValues.Name);

            if (any)
            {
                throw new InvalidOperationException();
            }

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
    }
}
