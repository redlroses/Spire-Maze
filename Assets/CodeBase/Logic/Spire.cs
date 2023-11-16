using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;
using CodeBase.StaticData;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Logic
{
    public class Spire : MonoBehaviour
    {
        private Level _level;
        private LevelStaticData _levelStaticData;

        public Cell GetLeftFrom(Cell from)
        {
            int floor = from.Id / _level.Width;
            int floorOffset = _level.Width * floor;
            int indexOnFloor = from.Id - floorOffset;
            int indexLeft = (indexOnFloor - 1).ClampRound(0, _level.Width);

            return _level.GetCell(_level.Height - floor - 1, indexLeft);
        }

        public Cell GetUpFrom(Cell from)
        {
            int floor = from.Id / _level.Width;
            int floorOffset = _level.Width * floor;
            int indexOnFloor = from.Id - floorOffset;
            int indexUp = floor + 1;

            return _level.GetCell(_level.Height - indexUp - 1, indexOnFloor);
        }

        public void Construct(Level level)
        {
            _level = level;
        }
    }
}