using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.LevelSpecification.Cells;
using UnityEngine;
using Door = CodeBase.LevelSpecification.Cells.Door;
using EnemySpawnPoint = CodeBase.LevelSpecification.Cells.EnemySpawnPoint;
using FinishPortal = CodeBase.LevelSpecification.Cells.FinishPortal;
using FireTrap = CodeBase.LevelSpecification.Cells.FireTrap;
using ItemSpawnPoint = CodeBase.LevelSpecification.Cells.ItemSpawnPoint;
using Key = CodeBase.LevelSpecification.Cells.Key;
using MovingPlate = CodeBase.LevelSpecification.Cells.MovingPlate;
using Object = UnityEngine.Object;
using Plate = CodeBase.LevelSpecification.Cells.Plate;
using Portal = CodeBase.LevelSpecification.Cells.Portal;
using Rock = CodeBase.LevelSpecification.Cells.Rock;
using Savepoint = CodeBase.LevelSpecification.Cells.Savepoint;
using SpikeTrap = CodeBase.LevelSpecification.Cells.SpikeTrap;
using Wall = CodeBase.LevelSpecification.Cells.Wall;

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

        public GameObject Instantiate(string path, Vector3 at, Transform inside)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity, inside);
        }

        public GameObject Instantiate(string path, Vector3 at, Quaternion rotation, Transform inside)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, rotation, inside);
        }

        public GameObject Instantiate(string path, Vector3 at, Quaternion rotation)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, rotation);
        }

        public GameObject Instantiate(string path, Vector3 at)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public GameObject Instantiate(string path, Transform inside)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, inside);
        }

        public GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public TType LoadAsset<TType>(string path) where TType : Object
        {
            TType sprite = Resources.Load<TType>(path);
            return sprite;
        }

        public void Cleanup()
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
                .ToDictionary(x => GetTypeByName(x.name), x => x);
        }

        private Type GetTypeByName(string name)
        {
            return name switch
            {
                nameof(Plate) => typeof(Plate),
                nameof(Wall) => typeof(Wall),
                nameof(WallHole) => typeof(WallHole),
                nameof(Door) => typeof(Door),
                nameof(Key) => typeof(Key),
                nameof(MovingPlate) => typeof(MovingPlate),
                nameof(MovingPlateMarker) => typeof(MovingPlateMarker),
                nameof(Portal) => typeof(Portal),
                nameof(SpikeTrap) => typeof(SpikeTrap),
                nameof(FireTrap) => typeof(FireTrap),
                nameof(Rock) => typeof(Rock),
                nameof(Savepoint) => typeof(Savepoint),
                nameof(EnemySpawnPoint) => typeof(EnemySpawnPoint),
                nameof(ItemSpawnPoint) => typeof(ItemSpawnPoint),
                nameof(FinishPortal) => typeof(FinishPortal),
                _ => throw new ArgumentException()
            };
        }
    }
}