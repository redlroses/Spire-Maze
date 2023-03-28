using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.LevelSpecification.Cells;
using UnityEngine;
// using Door = CodeBase.LevelSpecification.Cells.Door;
// using Key = CodeBase.LevelSpecification.Cells.Key;
// using MovingPlate = CodeBase.LevelSpecification.Cells.MovingPlate;
using Object = UnityEngine.Object;
// using Plate = CodeBase.LevelSpecification.Cells.Plate;
// using Portal = CodeBase.LevelSpecification.Cells.Portal;
// using Wall = CodeBase.LevelSpecification.Cells.Wall;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private Dictionary<Type, GameObject> _cellsPrefabs;

        public GameObject InstantiateCell<TCell>(Transform container) where TCell : Cell
        {
            if (_cellsPrefabs.TryGetValue(typeof(TCell), out GameObject cell))
            {
                return Object.Instantiate(cell, container);
            }

            throw new NullReferenceException($"There is no prefab for cell type: {typeof(TCell)}");
        }

        public GameObject Instantiate(string path, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public void CleanUp()
        {
            _cellsPrefabs = null;
        }

        public void LoadCells()
        {
            if (_cellsPrefabs is null == false)
            {
                return;
            }

            _cellsPrefabs = Resources
                .LoadAll<GameObject>(AssetPath.Cells)
                .ToDictionary(x => Compare(x.name), x => x);
        }

        private Type Compare(string name)
        {
            return name switch
            {
                nameof(Plate) => typeof(Plate),
                nameof(Wall) => typeof(Wall),
                nameof(Door) => typeof(Door),
                nameof(Key) => typeof(Key),
                nameof(MovingPlate) => typeof(MovingPlate),
                nameof(MovingPlateMarker) => typeof(MovingPlateMarker),
                nameof(Portal) => typeof(Portal),
                nameof(SpikeTrap) => typeof(SpikeTrap),
                nameof(FireTrap) => typeof(FireTrap),
                nameof(Savepoint) => typeof(Savepoint),
                _ => throw new ArgumentException()
            };
        }
    }
}