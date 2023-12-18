using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.Randomizer;
using UnityEngine;

namespace CodeBase.LevelSpecification.Constructor
{
    public class WallConstructor : ICellConstructor
    {
        private readonly IRandomService _randomService;

        public WallConstructor(IRandomService randomService)
        {
            _randomService = randomService;
        }

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell
        {
            for (int index = 0; index < cells.Length; index++)
            {
                Cell cell = cells[index];
                GameObject cellObject = gameFactory.CreateCell<TCell>(cell.Container);
                EnvironmentRandomizer randomizer = cellObject.GetComponent<EnvironmentRandomizer>();
                randomizer.Construct(_randomService);
                randomizer.EnableRandom();
            }
        }
    }
}