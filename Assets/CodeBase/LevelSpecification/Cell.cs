using CodeBase.Logic.Lift;
using CodeBase.Tools.Constants;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public class Cell
    {
        public CellType CellType;
        public Transform Container;
        public CellPosition CellPosition;

        public Cell(CellType cellType, Transform container)
        {
            CellType = cellType;
            Container = container;
            CellPosition = new CellPosition(container.position.y, container.rotation.eulerAngles.y);
        }

        public void AddTwoPiToAngle()
        {
            CellPosition = new CellPosition(CellPosition.Height, CellPosition.Angle + Trigonometry.TwoPiGrade);
        }

        public void RemoveTwoPiFromAngle()
        {
            CellPosition = new CellPosition(CellPosition.Height, CellPosition.Angle - Trigonometry.TwoPiGrade);
        }
    }
}
