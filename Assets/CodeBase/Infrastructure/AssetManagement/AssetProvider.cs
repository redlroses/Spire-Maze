using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly IPersistentProgressService _progressService;
        private readonly IPauseService _pauseService;
        private readonly Dictionary<string, Object> _cache = new Dictionary<string, Object>();

        public AssetProvider(IPersistentProgressService progressService, IPauseService pauseService)
        {
            _progressService = progressService;
            _pauseService = pauseService;
        }

        public GameObject InstantiateCell<TCell>(Transform container)
            where TCell : Cell
        {
            string cellPath = AssetPath.Combine(AssetPath.Cells, typeof(TCell).Name);
            GameObject gameObject = InstantiateRegistered(cellPath, container);
            RegisterPauseWatchers(gameObject);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        public GameObject InstantiateRegistered(string path, Vector3 at, Transform inside)
        {
            GameObject gameObject = Instantiate(path, at, Quaternion.identity, inside);
            RegisterProgressWatchers(gameObject);
            RegisterPauseWatchers(gameObject);

            return gameObject;
        }

        public GameObject InstantiateRegistered(string path, Vector3 at, Quaternion rotation, Transform inside)
        {
            GameObject gameObject = Instantiate(path, at, rotation, inside);
            RegisterProgressWatchers(gameObject);
            RegisterPauseWatchers(gameObject);

            return gameObject;
        }

        public GameObject InstantiateRegistered(string path, Vector3 at, Quaternion rotation)
        {
            GameObject gameObject = Instantiate(path, at, rotation);
            RegisterProgressWatchers(gameObject);
            RegisterPauseWatchers(gameObject);

            return gameObject;
        }

        public GameObject InstantiateRegistered(string path, Vector3 at)
        {
            GameObject gameObject = Instantiate(path, at);
            RegisterProgressWatchers(gameObject);
            RegisterPauseWatchers(gameObject);

            return gameObject;
        }

        public GameObject InstantiateRegistered(string path, Transform inside)
        {
            GameObject gameObject = Instantiate(path, inside);
            RegisterProgressWatchers(gameObject);
            RegisterPauseWatchers(gameObject);

            return gameObject;
        }

        public GameObject InstantiateRegistered(string path)
        {
            GameObject gameObject = Instantiate(path);
            RegisterProgressWatchers(gameObject);
            RegisterPauseWatchers(gameObject);

            return gameObject;
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

        public TType LoadAsset<TType>(string path)
            where TType : Object
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

        private T LoadCached<T>(string path)
            where T : Object
        {
            if (_cache.TryGetValue(path, out Object cached))
                return cached as T;

            T loaded = Resources.Load<T>(path);
            _cache.Add(path, loaded);

            return loaded;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                _progressService.Register(progressReader);
        }

        private void RegisterPauseWatchers(GameObject gameObject)
        {
            foreach (IPauseWatcher pauseWatcher in gameObject.GetComponentsInChildren<IPauseWatcher>())
                _pauseService.Register(pauseWatcher);
        }
    }
}