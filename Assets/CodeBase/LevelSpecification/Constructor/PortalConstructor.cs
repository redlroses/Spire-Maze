using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Portal;
using Portal = CodeBase.EditorCells.Portal;

namespace CodeBase.LevelSpecification.Constructor
{
    public class PortalConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
            where TCell : Cell
        {
            List<Cell> portals = new List<Cell>(cells);

            foreach (var cell in cells)
            {
                gameFactory.CreateCell<TCell>(cell.Container).GetComponent<PortalGate>();
            }

            while (portals.Count > 0)
            {
                Cell currentPortal = portals[0];
                Portal currentPortalData = (Portal) currentPortal.CellData;
                Cell linkedPortal = FindLinked(portals, currentPortalData);
                PortalGate currentPortalGate = currentPortal.Container.GetComponentInChildren<PortalGate>();
                PortalGate linkedPortalGate = linkedPortal.Container.GetComponentInChildren<PortalGate>();
                currentPortalGate.Construct(currentPortal.Id, linkedPortalGate);
                linkedPortalGate.Construct(linkedPortal.Id, currentPortalGate);
                portals.Remove(currentPortal);
                portals.Remove(linkedPortal);
            }
        }

        private Cell FindLinked(IEnumerable<Cell> portals, Portal currentPortalData)
        {
            return portals.First(portal =>
                ((Portal) portal.CellData).Key == currentPortalData.Key &&
                currentPortalData != (Portal) portal.CellData);
        }
    }
}