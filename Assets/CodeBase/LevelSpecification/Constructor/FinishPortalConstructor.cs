using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using FinishPortal = CodeBase.Logic.FinishPortal;

namespace CodeBase.LevelSpecification.Constructor
{
    public class FinishPortalConstructor : ICellConstructor
    {
        private readonly IWindowService _windowService;

        public FinishPortalConstructor(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void Construct<TCell>(IGameFactory gameFactory, IStaticDataService staticData, Cell[] cells) where TCell : Cell
        {
            foreach (var cell in cells)
            {
                gameFactory.CreateCell<TCell>(cell.Container).GetComponent<FinishPortal>().Construct(_windowService);
                Debug.Log("finish");
            }
        }
    }
}