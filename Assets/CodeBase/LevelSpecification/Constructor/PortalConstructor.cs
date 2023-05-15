﻿using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Portal;

namespace CodeBase.LevelSpecification.Constructor
{
    public class PortalConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
            where TCell : Cell
        {
            var portals = new List<Cell>(cells);

            foreach (var cell in cells)
            {
                gameFactory.CreateCell<TCell>(cell.Container).GetComponent<PortalGate>();
            }

            while (portals.Count > 0)
            {
                var currentPortal = portals[0];
                var currentPortalData = (EditorCells.Portal) currentPortal.CellData;
                var linkedPortal = FindLinked(portals, currentPortalData);
                var currentPortalGate = currentPortal.Container.GetComponentInChildren<PortalGate>();
                var linkedPortalGate = linkedPortal.Container.GetComponentInChildren<PortalGate>();
                currentPortalGate.Construct(currentPortal.Id, linkedPortalGate);
                linkedPortalGate.Construct(linkedPortal.Id, currentPortalGate);
                portals.Remove(currentPortal);
                portals.Remove(linkedPortal);
            }
        }

        private Cell FindLinked(IEnumerable<Cell> portals, EditorCells.Portal currentPortalData)
        {
            return portals.First(portal =>
                ((EditorCells.Portal) portal.CellData).Key == currentPortalData.Key &&
                currentPortalData != (EditorCells.Portal) portal.CellData);
        }
    }
}