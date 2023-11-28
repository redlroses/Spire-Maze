using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic;
using CodeBase.Logic.Items;
using CodeBase.Services.Cameras;
using CodeBase.Services.Input;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.Watch;
using CodeBase.StaticData.Storable;
using CodeBase.Tools.Extension;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Compass = CodeBase.Logic.Items.Compass;
using Key = CodeBase.Logic.Items.Key;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory, IHeroLocator
    {
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private readonly IAssetProvider _assets;
        private readonly IPersistentProgressService _progressService;
        private readonly IPauseService _pauseService;
        private readonly IUIFactory _uiFactory;
        private readonly IInputService _inputService;
        private readonly ICameraOperatorService _cameraOperator;

        private readonly Dictionary<string, Material> _materials;
        private readonly Dictionary<string, PhysicMaterial> _physicMaterials;

        private Transform _heroTransform;

        public Transform Location => _heroTransform;

        public GameFactory(IAssetProvider assets, IPersistentProgressService progressService,
            IPauseService pauseService, IUIFactory uiFactory, IInputService inputService,
            ICameraOperatorService cameraOperator)
        {
            _assets = assets;
            _progressService = progressService;
            _pauseService = pauseService;
            _uiFactory = uiFactory;
            _inputService = inputService;
            _cameraOperator = cameraOperator;
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

        public GameObject CreateHero(Vector3 at)
        {
            GameObject hero = InstantiateRegistered(AssetPath.Hero, at);
            _heroTransform = hero.transform;
            return hero;
        }

        public GameObject CreateCell<TCell>(Transform container) where TCell : Cell
        {
            GameObject cell = _assets.InstantiateCell<TCell>(container);
            RegisterProgressWatchers(cell);
            RegisterPauseWatchers(cell);

            return cell;
        }

        public GameObject CreateLobby() =>
            InstantiateRegistered(AssetPath.Lobby);

        public GameObject CreateSpireSegment(Vector3 at, Quaternion rotation) =>
            _assets.Instantiate(AssetPath.SpireSegment, at, rotation);

        public GameObject CreateHorizontalRail(Transform container) =>
            _assets.Instantiate(AssetPath.HorizontalRail, Vector3.zero.ChangeY(container.position.y), container.rotation, container);

        public GameObject CreateVerticalRail(Transform container) =>
            _assets.Instantiate(AssetPath.VerticalRail, container);

        public GameObject CreateHorizontalRailLock(Transform container) =>
            _assets.Instantiate(AssetPath.HorizontalRailLock, Vector3.zero.ChangeY(container.position.y), container.rotation, container);

        public GameObject CreateVerticalRailLock(Transform container) =>
            _assets.Instantiate(AssetPath.VerticalRailLock, container);

        public GameObject CreateTutorialTrigger(Transform container) =>
            _assets.Instantiate(AssetPath.TutorialTrigger, container);

        public IItem CreateItem(StorableStaticData data) =>
            data.ItemType switch
            {
                StorableType.Compass => new Compass(data, _progressService, _uiFactory, this),
                StorableType.Binocular => new Binocular(data, _uiFactory, _inputService, this, _cameraOperator, _progressService),
                StorableType.BlueKey => new Key(data),
                StorableType.RedKey => new Key(data),
                StorableType.GreenKey => new Key(data),
                StorableType.RuneAlpha => new Rune(data),
                StorableType.RuneBeta => new Rune(data),
                StorableType.RuneGamma => new Rune(data),
                StorableType.RuneDelta => new Rune(data),
                StorableType.RuneEpsilon => new Rune(data),
                StorableType.None => throw new Exception("Storable type is not specified"),
                _ => new Item(data)
            };

        public GameObject CreateVirtualMover() =>
            _assets.Instantiate(AssetPath.VirtualMover, _heroTransform.position);

        public Material CreateMaterial(string name) =>
            GetCashed(name, AssetPath.Materials, _materials);

        public PhysicMaterial CreatePhysicMaterial(string name) =>
            GetCashed(name, AssetPath.PhysicMaterials, _physicMaterials);

        public GameObject CreateMusicPlayer() =>
            _assets.Instantiate(AssetPath.MusicPlayer);

        public GameObject CreateHud() =>
            InstantiateRegistered(AssetPath.Hud);

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

            TValue loaded = Resources.Load<TValue>($"{path}/{key}");

            if (loaded is null)
                throw new NullReferenceException($"There is no object with key {key}");

            collection.Add(key, loaded);
            return loaded;
        }
    }
}