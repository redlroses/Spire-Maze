using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CodeBase.LevelSpecification.Cells;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    [SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
    public sealed class Level : CellContainer<Floor>, IEnumerable<Cell>, IEnumerator<Cell>
    {
        private int _currentIndex = -1;

        public Level(int size, Transform origin, List<Floor> container = null)
            : base(size, origin, container)
        {
        }

        public int Height => Container.Count;

        public int Width => Container[0].Container.Count;

        public int Size => Height * Width;

        public object Current => GetCell(_currentIndex / Width, _currentIndex % Width);

        Cell IEnumerator<Cell>.Current => GetCell(_currentIndex / Width, _currentIndex % Width);

        bool IEnumerator.MoveNext() =>
            ++_currentIndex < Size;

        void IEnumerator.Reset() =>
            _currentIndex = -1;

        IEnumerator<Cell> IEnumerable<Cell>.GetEnumerator()
        {
            ((IEnumerator)this).Reset();

            return this;
        }

        public IEnumerator GetEnumerator() => this;

        public void Dispose()
        {
        }

        public Cell GetCell(int floor, int onFloorIndex) =>
            Container[floor].Container[onFloorIndex];

        public Cell GetCell(int index) =>
            Container[(Height - (index / Width)) - 1].Container[index % Width];
    }
}