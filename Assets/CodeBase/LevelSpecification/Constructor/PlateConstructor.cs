﻿using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.LevelSpecification.Constructor
{
    public class PlateConstructor : ICellConstructor
    {
        public void Construct<TCell>(Cell[] cells)
        {
            foreach (var cell in cells)
            {
                CellFactory.InstantiatePlate(cell.Container);
                PlateExampleSetup();
            }
        }

        private void PlateExampleSetup()
        {
            Debug.Log("PlateSetup");
        }
    }
}