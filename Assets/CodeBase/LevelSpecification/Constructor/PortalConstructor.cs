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
                EditorCells.Portal currentPortalData = (EditorCells.Portal) currentPortal.CellData;
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

        private Cell FindLinked(IEnumerable<Cell> portals, EditorCells.Portal currentPortalData)
        {
            return portals.First(portal =>
                ((EditorCells.Portal) portal.CellData).Key == currentPortalData.Key &&
                currentPortalData != (EditorCells.Portal) portal.CellData);
        }
    }
}