using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Portal;

namespace CodeBase.LevelSpecification.Constructor
{
    public class PortalConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
        {
            List<Cell> portals = new List<Cell>(cells);

            foreach (var cell in cells)
            {
                gameFactory.CreateCell<Portal>(cell.Container).GetComponent<PortalGate>();
            }

            while (portals.Count > 0)
            {
                Cell currentPortal = portals[0];
                Data.Cell.Portal currentPortalData = (Data.Cell.Portal) currentPortal.CellData;
                Cell linkedPortal = FindLinked(portals, currentPortalData);
                PortalGate currentPortalGate = currentPortal.Container.GetComponentInChildren<PortalGate>();
                PortalGate linkedPortalGate = linkedPortal.Container.GetComponentInChildren<PortalGate>();
                currentPortalGate.Construct(linkedPortalGate);
                linkedPortalGate.Construct(currentPortalGate);
                portals.Remove(currentPortal);
                portals.Remove(linkedPortal);
            }

            // foreach (var cell in cells)
            // {
            //     Data.Cell.Portal currentPortalData = (Data.Cell.Portal) cell.CellData;
            //     Cell linked = FindLinked(cells, currentPortalData);
            //     PortalGate currentPortalGate = cell.Container.GetComponentInChildren<PortalGate>();
            //     PortalGate linkedPortalGate = linked.Container.GetComponentInChildren<PortalGate>();
            //     currentPortalGate.Construct(linkedPortalGate);
            //     linkedPortalGate.Construct(currentPortalGate);
            // }
        }

        private Cell FindLinked(IEnumerable<Cell> portals, Data.Cell.Portal currentPortalData)
        {
            return portals.First(portal =>
                ((Data.Cell.Portal) portal.CellData).Key == currentPortalData.Key &&
                currentPortalData != (Data.Cell.Portal) portal.CellData);
        }
    }
}