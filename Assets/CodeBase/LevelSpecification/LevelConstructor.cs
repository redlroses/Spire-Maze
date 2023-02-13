using System.Linq;
using CodeBase.LevelSpecification.Cells;
using CodeBase.LevelSpecification.Constructor;
using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public class LevelConstructor : MonoBehaviour
    {
        private readonly CellConstructor _cellConstructor = new CellConstructor();

        public void Construct(Level level)
        {
            _cellConstructor.Construct<Wall>(level.Where(cell => cell.CellType == CellType.Wall).ToArray());
            _cellConstructor.Construct<Plate>(level.Where(cell => cell.CellType == CellType.Plate).ToArray());
        }
    }
}