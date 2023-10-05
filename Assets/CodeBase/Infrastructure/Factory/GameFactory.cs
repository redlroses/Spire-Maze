using System;
using System.Collections.Generic;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic;
using CodeBase.Logic.Items;
using CodeBase.Services.Input;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.Watch;
using CodeBase.StaticData.Storable;
using CodeBase.UI.Elements;
using CodeBase.UI.Elements.Buttons;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory, IHeroLocator
    {
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private readonly IAssetProvider _assets;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IPauseService _pauseService;
        private readonly IWindowService _windowService;
        private readonly IWatchService _watchService;
        private readonly IUIFactory _uiFactory;
        private readonly IPlayerInputService _inputService;

        private readonly Dictionary<Colors, Material> _coloredMaterials;
        private readonly Dictionary<string, Material> _materials;
        private readonly Dictionary<string, PhysicMaterial> _physicMaterials;

        private Transform _heroTransform;

        public Transform Location => _heroTransform;

        public GameFactory(IAssetProvider assets, IRandomService randomService,
            IPersistentProgressService persistentProgressService, IPauseService pauseService,
            IWindowService windowService, IWatchService watchService, IUIFactory uiFactory, IPlayerInputService inputService)
        {
            _assets = assets;
            _randomService = randomService;
            _persistentProgressService = persistentProgressService;
            _pauseService = pauseService;
            _windowService = windowService;
            _watchService = watchService;
            _uiFactory = uiFactory;
            _inputService = inputService;
            _coloredMaterials = new Dictionary<Colors, Material>(4);
            _materials = new Dictionary<string, Material>(4);
            _physicMaterials = new Dictionary<string, PhysicMaterial>(2);
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public void WarmUp() =>
            _assets.LoadCells();

        public Material CreateColoredMaterial(Colors color) =>
            GetCashed(color, AssetPath.Materials, _coloredMaterials);

        public GameObject CreateSpire() =>
            _assets.Instantiate(path: AssetPath.Spire);

        public GameObject CreateHero(Vector3 at)
        {
            GameObject hero = InstantiateRegistered(prefabPath: AssetPath.HeroPath, at);
            _heroTransform = hero.transform;
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

        public IItem CreateItem(StorableStaticData data) =>
            data.ItemType switch
            {
                StorableType.Compass => new CompassItem(data, _uiFactory, this),
                StorableType.Binocular => new BinocularsItem(data, _uiFactory, _inputService, this),
                StorableType.BlueKey => new KeyItem(data),
                StorableType.RedKey => new KeyItem(data),
                StorableType.GreenKey => new KeyItem(data),
                StorableType.None => throw new Exception("Storable type is not specified"),
                _ => new Item(data)
            };

        public GameObject CreateVirtualMover() =>
            _assets.Instantiate(AssetPath.VirtualMover, _heroTransform.position);

        public Material CreateMaterial(string name) =>
            GetCashed(name, AssetPath.Materials, _materials);

        public PhysicMaterial CreatePhysicMaterial(string name) =>
            GetCashed(name, AssetPath.PhysicMaterials, _physicMaterials);

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);
            hud.GetComponentInChildren<PauseToggle>().Construct(_pauseService, _windowService);
            hud.GetComponentInChildren<ClockText>().Construct(_watchService);
            hud.GetComponentInChildren<ExtraLivesBarView>().Construct(_uiFactory);
            hud.GetComponentInChildren<LeaderboardButton>().Construct(_windowService);
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

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void RegisterPauseWatchers(GameObject gameObject)
        {
            foreach (IPauseWatcher pauseWatcher in gameObject.GetComponentsInChildren<IPauseWatcher>())
                _pauseService.Register(pauseWatcher);
        }

        private TValue GetCashed<TKey, TValue>(TKey key, string path, Dictionary<TKey, TValue> collection)
            where TValue : Object
        {
            if (collection.TryGetValue(key, out TValue value))
                return value;

            var loaded = Resources.Load<TValue>($"{path}/{key}");

            if (loaded is null)
                throw new NullReferenceException($"There is no object with key {key}");

            collection.Add(key, loaded);
            return loaded;
        }
    }
}