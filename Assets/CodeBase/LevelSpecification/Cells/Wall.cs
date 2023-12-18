using CodeBase.EditorCells;
using UnityEngine;

namespace CodeBase.LevelSpecification.Cells
{
    public class Wall : Cell
    {
        public Wall(CellData cellData, Transform container, int id)
            : base(cellData, container, id)
        {
        }
    }
}