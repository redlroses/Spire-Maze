using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public abstract class CellContainer<T>
    {
        public readonly List<T> Container;
        public readonly Transform Origin;

        protected CellContainer(int size, Transform origin, List<T> container = null)
        {
            Origin = origin;
            Container = container ?? new List<T>(size);
        }

        public void Add(T item)
        {
            Container.Add(item);
        }
    }
}