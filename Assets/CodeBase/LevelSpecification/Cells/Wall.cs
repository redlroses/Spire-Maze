using CodeBase.Data.Cell;
using UnityEngine;

namespace CodeBase.LevelSpecification.Cells
{
    public class Wall : Cell
    {
        public Wall(CellData cellData, Transform container) : base(cellData, container)
        {
        }
    }
}