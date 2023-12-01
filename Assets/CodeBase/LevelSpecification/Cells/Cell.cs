using CodeBase.EditorCells;
using CodeBase.Logic;
using CodeBase.Logic.Lift;
using CodeBase.Tools.Constants;
using UnityEngine;

namespace CodeBase.LevelSpecification.Cells
{
    public class Cell
    {
        public readonly CellData CellData;
        public readonly Transform Container;

        private CellPosition _position;

        public int Id { get; }
        public CellPosition Position => _position;

        public Cell(CellData cellData, Transform container, int id)
        {
            CellData = cellData;
            Container = container;
            _position = new CellPosition(container.position.y, container.rotation.eulerAngles.y);
            Id = id;
        }

        public void AddTwoPiToAngle() =>
            _position = new CellPosition(_position.Height, _position.Angle + Trigonometry.TwoPiGrade);

        public void RemoveTwoPiFromAngle() =>
            _position = new CellPosition(_position.Height, _position.Angle - Trigonometry.TwoPiGrade);
    }
}
