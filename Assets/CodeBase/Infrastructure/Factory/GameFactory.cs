using System;
using System.Collections.Generic;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Movement;
using CodeBase.Logic.Player;
using CodeBase.Services.Input;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.Score;
using CodeBase.Services.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Object = UnityEngine.Object;

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
        private readonly IPauseService _pauseService;
        private readonly IScoreService _scoreService;
        private readonly IWindowService _windowService;
        private readonly IPlayerInputService _inputService;

        private readonly Dictionary<Colors, Material> _coloredMaterials;
        private readonly Dictionary<string, Material> _materials;
        private readonly Dictionary<string, PhysicMaterial> _physicMaterials;

        public GameFactory(IAssetProvider assets, IStaticDataService staticData, IRandomService randomService,
            IPersistentProgressService persistentProgressService, IPauseService pauseService, IScoreService scoreService, IWindowService windowService, IPlayerInputService inputService)
        {
            _inputService = inputService;
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _persistentProgressService = persistentProgressService;
            _scoreService = scoreService;
            _pauseService = pauseService;
            _windowService = windowService;
            _coloredMaterials = new Dictionary<Colors, Material>((int) Colors.Rgb);
            _materials = new Dictionary<string, Material>(4);
            _physicMaterials = new Dictionary<string, PhysicMaterial>(2);
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public void WarmUp()
        {
            _assets.LoadCells();
        }

        public Material CreateColoredMaterial(Colors color) =>
            GetCashed(color, AssetPath.Materials, _coloredMaterials);

        public GameObject CreateSpire() =>
            _assets.Instantiate(path: AssetPath.Spire);

        public GameObject CreateHero(Vector3 at)
        {
            var hero = InstantiateRegistered(prefabPath: AssetPath.HeroPath, at);
            hero.GetComponent<HeroMover>().Construct(_inputService);
            hero.GetComponent<Jumper>().Construct(_inputService);
            hero.GetComponentInChildren<Dodge>().Construct(_inputService);
            hero.GetComponent<Hero>().Construct(_inputService);
            return hero;
        }

        public GameObject CreateEnemy(string prefabPath, Vector3 position) =>
            InstantiateRegistered(prefabPath, position);

        public GameObject CreateCell<TCell>(Transform container) where TCell : Cell
        {
            GameObject cell = _assets.InstantiateCell<TCell>(container);
            RegisterProgressWatchers(cell);
            RegisterPauseWatchers(cell);
            return cell;
        }

        public GameObject CreateLobby() =>
            InstantiateRegistered(AssetPath.Lobby);

        public Material CreateMaterial(string name) =>
            GetCashed(name, AssetPath.Materials, _materials);

        public PhysicMaterial CreatePhysicMaterial(string name) =>
            GetCashed(name, AssetPath.PhysicMaterials, _physicMaterials);

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);
            hud.GetComponentInChildren<PauseToggle>().Construct(_pauseService, _windowService);
            return hud;
        }

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
            RegisterPauseWatchers(gameObject);

            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(path: prefabPath);
            RegisterProgressWatchers(gameObject);
            RegisterPauseWatchers(gameObject);

            return gameObject;
        }

        private void RegisterPauseWatchers(GameObject gameObject)
        {
            foreach (IPauseWatcher pauseWatcher in gameObject.GetComponentsInChildren<IPauseWatcher>())
            {
                pauseWatcher.RegisterPauseWatcher(_pauseService);
            }
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }

        private TValue GetCashed<TKey, TValue>(TKey key, string path, Dictionary<TKey, TValue> collection)
            where TValue : Object
        {
            if (collection.TryGetValue(key, out TValue value))
            {
                return value;
            }

            TValue loaded = Resources.Load<TValue>($"{path}/{key}");

            if (loaded is null)
            {
                throw new NullReferenceException($"There is no object with key {key}");
            }

            collection.Add(key, loaded);
            return loaded;
        }
    }
}