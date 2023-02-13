using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public sealed class Level : CellContainer<Floor>, IEnumerable<Cell>, IEnumerator<Cell>
    {
        private int _currentIndex;

        public int Height => Container.Count;
        public int Width => Container[0].Container.Count;
        public int Size => Height * Width;

        public object Current => GetCell(_currentIndex / Width, _currentIndex % Width);
        Cell IEnumerator<Cell>.Current => GetCell(_currentIndex / Width, _currentIndex % Width);

        public Level(Transform selfTransform, int size) : base(selfTransform, size)
        {
        }

        public bool MoveNext()
        {
            _currentIndex++;
            return _currentIndex < Size;
        }

        public void Reset()
        {
            _currentIndex = 0;
            MoveNext();
        }

        IEnumerator<Cell> IEnumerable<Cell>.GetEnumerator() =>
            this;

        public IEnumerator GetEnumerator() =>
            this;

        public void Dispose()
        {
        }

        public Cell GetCell(int floor, int index) =>
            Container[floor].Container[index];
    }
}