using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public abstract class CellContainer<T>
    {
        public readonly List<T> Container;

        protected CellContainer(Transform selfTransform, int size)
        {
            Container = new List<T>(size);
        }

        public void Add(T item)
        {
            Container.Add(item);
        }
    }
}