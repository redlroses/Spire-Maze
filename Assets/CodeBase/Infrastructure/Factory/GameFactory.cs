using System;
using System.Collections.Generic;
using CodeBase.Data.Cell;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly Dictionary<Colors, Material> _coloredMaterials;

        private GameObject _heroGameObject;

        public GameFactory(IAssetProvider assets, IStaticDataService staticData, IRandomService randomService,
            IPersistentProgressService persistentProgressService)
        {
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _persistentProgressService = persistentProgressService;
            _coloredMaterials = new Dictionary<Colors, Material>((int) Colors.Rgb);
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();

            _heroGameObject = null;
        }

        public void WarmUp()
        {
            _assets.LoadCells();
        }

        public Material CreateColoredMaterial(Colors color)
        {
            if (_coloredMaterials.TryGetValue(color, out Material material))
            {
                return material;
            }

            Material loaded = Resources.Load<Material>($"{AssetPath.Materials}/{color.ToString()}");

            if (loaded is null)
            {
                throw new NullReferenceException($"There is no material for color {color}");
            }

            _coloredMaterials.Add(color, loaded);
            return loaded;
        }

        public GameObject CreateSpire() =>
            _assets.Instantiate(path: AssetPath.Spire);

        public GameObject CreateHero(Vector3 at) =>
            InstantiateRegistered(prefabPath: AssetPath.HeroPath, at);

        public GameObject CreateCell<TCell>(Transform container) where TCell : Cell
        {
            GameObject cell = _assets.InstantiateCell<TCell>(container);
            RegisterProgressWatchers(cell);
            return cell;
        }

        public Material CreateMaterial(string name) =>
            Resources.Load<Material>(path: $"{AssetPath.Materials}/{name}");

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assets.Instantiate(path: prefabPath, at: at);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            _heroGameObject ??= _assets.Instantiate(path: prefabPath);
            RegisterProgressWatchers(_heroGameObject);

            return _heroGameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }
    }
}