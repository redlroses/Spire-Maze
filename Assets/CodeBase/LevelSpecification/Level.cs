using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public sealed class Level : CellContainer<Floor>
    {
        public int Height => Container.Count;
        public int Width => Container[0].Container.Count;
        public int Size => Height * Width;

        public Level(Transform selfTransform, int size) : base(selfTransform, size)
        {
        }

        public Cell GetCell(int floor, int index) =>
            Container[floor].Container[index];
    }
}