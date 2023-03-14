﻿using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Сollectible;

namespace CodeBase.LevelSpecification.Constructor
{
    public class KeyConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell
        {
            foreach (var cell in cells)
            {
                var keyData = (EditorCells.Key) cell.CellData;
                KeyCollectible key = gameFactory.CreateCell<TCell>(cell.Container).GetComponent<KeyCollectible>();
                key.Construct(gameFactory, keyData.Color, cell.Id);
            }
        }
    }
}