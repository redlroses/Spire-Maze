using UnityEngine;

namespace CodeBase.LevelSpecification.Cells
{
    public class Wall : Cell
    {
        public Wall(CellType cellType, Transform container) : base(cellType, container)
        {
        }
    }
}