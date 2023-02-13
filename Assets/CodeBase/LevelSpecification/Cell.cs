using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public class Cell
    {
        public CellType CellType;
        public Transform Container;

        public Cell(CellType cellType, Transform container)
        {
            CellType = cellType;
            Container = container;
        }
    }
}
