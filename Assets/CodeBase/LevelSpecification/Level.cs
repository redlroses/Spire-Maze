using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.EditorCells;
using CodeBase.LevelSpecification.Cells;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public sealed class Level : CellContainer<Floor>, IEnumerable<Cell>, IEnumerator<Cell>
    {
        private int _currentIndex = -1;

        public int Height => Container.Count;
        public int Width => Container[0].Container.Count;
        public int Size => Height * Width;

        public object Current => GetCell(_currentIndex / Width, _currentIndex % Width);
        Cell IEnumerator<Cell>.Current => GetCell(_currentIndex / Width, _currentIndex % Width);

        public Level(int size, Transform selfTransform, List<Floor> container = null) : base(size, selfTransform, container)
        {
        }

        public bool MoveNext()
        {
            _currentIndex++;
            return _currentIndex < Size;
        }

        public void Reset()
        {
            _currentIndex = -1;
            MoveNext();
        }

        IEnumerator<Cell> IEnumerable<Cell>.GetEnumerator() =>
            new Level(Size, SelfTransform, Container);

        public IEnumerator GetEnumerator() =>
            this;

        public void Dispose()
        {
        }

        public Cell GetCell(int floor, int index) =>
            Container[floor].Container[index];
    }
}