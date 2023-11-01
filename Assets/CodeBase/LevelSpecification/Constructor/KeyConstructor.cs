﻿using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Сollectible;
using CodeBase.Services.StaticData;
using CodeBase.Tools.Extension;
using UnityEngine;
using Key = CodeBase.EditorCells.Key;

namespace CodeBase.LevelSpecification.Constructor
{
    public class KeyConstructor : ICellConstructor
    {
        private readonly IStaticDataService _staticDataService;

        public KeyConstructor(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells) where TCell : Cell
        {
            foreach (var cell in cells)
            {
                var keyData = (Key) cell.CellData;
                Collectible key = gameFactory.CreateCell<TCell>(cell.Container).GetComponent<Collectible>();
                key.Construct(cell.Id, gameFactory.CreateItem(_staticDataService.ForStorable(keyData.Color.ToStorableType())));
            }
        }
    }
}