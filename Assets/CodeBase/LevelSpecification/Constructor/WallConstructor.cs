using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.LevelSpecification.Constructor
{
    public class WallConstructor : ICellConstructor
    {
        public void Construct<TCell>(Cell[] cells)
        {
            foreach (var cell in cells)
            {
                CellFactory.InstantiateCell<TCell>(cell.Container);
                PlateExampleSetup();
            }
        }

        private void PlateExampleSetup()
        {
            Debug.Log("PlateSetup");
        }
    }
}