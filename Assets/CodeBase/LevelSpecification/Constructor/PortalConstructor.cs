using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Portal;
using UnityEngine;
using Portal = CodeBase.EditorCells.Portal;

namespace CodeBase.LevelSpecification.Constructor
{
    public class PortalConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
            where TCell : Cell
        {
            List<Cell> portals = new List<Cell>(cells);

            foreach (Cell cell in cells)
            {
                gameFactory.CreateCell<TCell>(cell.Container).GetComponent<PortalGate>();
            }

            while (portals.Count > 0)
            {
                Cell currentPortalCell = portals[0];
                Portal currentPortalData = (Portal)currentPortalCell.CellData;
                Cell linkedPortalCell = FindLinked(portals, currentPortalData);
                LinkPortals(currentPortalCell, linkedPortalCell, currentPortalData.Color);
                portals.Remove(currentPortalCell);
                portals.Remove(linkedPortalCell);
            }
        }

        private void LinkPortals(Cell currentPortal, Cell linkedPortal, Color32 color)
        {
            PortalGate currentPortalGate = currentPortal.Container.GetComponentInChildren<PortalGate>();
            PortalGate linkedPortalGate = linkedPortal.Container.GetComponentInChildren<PortalGate>();

            currentPortalGate.Construct(currentPortal.Id, linkedPortalGate, color);
            linkedPortalGate.Construct(linkedPortal.Id, currentPortalGate, color);
        }

        private Cell FindLinked(IEnumerable<Cell> portals, Portal currentPortalData)
        {
            return portals.First(
                portal =>
                    ((Portal)portal.CellData).Key == currentPortalData.Key &&
                    currentPortalData != (Portal)portal.CellData);
        }
    }
}