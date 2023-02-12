using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public sealed class Floor : CellContainer<Cell>
    {
        public Floor(Transform selfTransform, int size) : base(selfTransform, size)
        {
        }
    }
}