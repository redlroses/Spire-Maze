using System.Collections.Generic;
using CodeBase.LevelSpecification.Cells;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public sealed class Floor : CellContainer<Cell>
    {
        public Floor(int size, Transform origin, List<Cell> container = null)
            : base(size, origin, container)
        {
        }
    }
}