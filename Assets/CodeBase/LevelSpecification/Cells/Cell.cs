using CodeBase.Data.Cell;
using CodeBase.Logic.Lift;
using CodeBase.Tools.Constants;
using UnityEngine;

namespace CodeBase.LevelSpecification.Cells
{
    public class Cell
    {
        public readonly CellData CellData;
        public readonly Transform Container;

        public CellPosition Position;

        public Cell(CellData cellData, Transform container)
        {
            CellData = cellData;
            Container = container;
            Position = new CellPosition(container.position.y, container.rotation.eulerAngles.y);
        }

        public void AddTwoPiToAngle()
        {
            Position = new CellPosition(Position.Height, Position.Angle + Trigonometry.TwoPiGrade);
        }

        public void RemoveTwoPiFromAngle()
        {
            Position = new CellPosition(Position.Height, Position.Angle - Trigonometry.TwoPiGrade);
        }
    }
}
