using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public abstract class CellContainer<T>
    {
        public readonly List<T> Container;
        public Transform SelfTransform;

        protected CellContainer(int size, Transform selfTransform, List<T> container = null)
        {
            SelfTransform = selfTransform;
            Container = container ?? new List<T>(size);
        }

        public void Add(T item)
        {
            Container.Add(item);
        }
    }
}