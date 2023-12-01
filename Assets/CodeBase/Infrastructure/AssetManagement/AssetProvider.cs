using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeBase.LevelSpecification.Cells;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, Object> _cache = new Dictionary<string, Object>();

        public GameObject InstantiateCell<TCell>(Transform container) where TCell : Cell
        {
            string cellPath = AssetPath.Combine(AssetPath.Cells, typeof(TCell).Name);
            GameObject asset = LoadCached<GameObject>(cellPath);
            return Object.Instantiate(asset, container);
        }

        public GameObject Instantiate(string path, Vector3 at, Transform inside)
        {
            GameObject prefab = LoadCached<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity, inside);
        }

        public GameObject Instantiate(string path, Vector3 at, Quaternion rotation, Transform inside)
        {
            GameObject prefab = LoadCached<GameObject>(path);
            return Object.Instantiate(prefab, at, rotation, inside);
        }

        public GameObject Instantiate(string path, Vector3 at, Quaternion rotation)
        {
            GameObject prefab = LoadCached<GameObject>(path);
            return Object.Instantiate(prefab, at, rotation);
        }

        public GameObject Instantiate(string path, Vector3 at)
        {
            GameObject prefab = LoadCached<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public GameObject Instantiate(string path, Transform inside)
        {
            GameObject prefab = LoadCached<GameObject>(path);
            return Object.Instantiate(prefab, inside);
        }

        public GameObject Instantiate(string path)
        {
            GameObject prefab = LoadCached<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public TType LoadAsset<TType>(string path) where TType : Object
        {
            TType sprite = LoadCached<TType>(path);
            return sprite;
        }

        public void PreloadCells()
        {
            IEnumerable<Type> cellTypes = typeof(Cell).Assembly.ExportedTypes.Where(t => t.BaseType == typeof(Cell));

            foreach (Type type in cellTypes)
            {
                string path = AssetPath.Combine(AssetPath.Cells, type.Name);
                ResourceRequest request = Resources.LoadAsync<GameObject>(path);
                request.completed += _ => _cache.TryAdd(path, request.asset);
            }
        }

        public void Cleanup()
        {
            _cache.Clear();

            foreach (KeyValuePair<string, Object> asset in _cache)
                Resources.UnloadAsset(asset.Value);
        }

        private T LoadCached<T>(string path) where T : Object
        {
            if (_cache.TryGetValue(path, out Object cached))
                return cached as T;

            T loaded = Resources.Load<T>(path);
            _cache.Add(path, loaded);
            return loaded;
        }
    }
}