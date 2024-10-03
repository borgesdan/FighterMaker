using FighterMaker.Visual.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FighterMaker.Visual.Core
{
    public class ActorModelCollection : ICollection<ActorModel>
    {
        readonly List<ActorModel> items = [];

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public void Add(ActorModel item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(ActorModel item)
        {
            return items.Contains(item);
        }

        public void CopyTo(ActorModel[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ActorModel> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public bool Remove(ActorModel item)
        {
            return items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
