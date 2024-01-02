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
using CodeBase.StaticData.Storable;
using CodeBase.Tools.Extension;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using Compass = CodeBase.Logic.Items.Compass;
using Key = CodeBase.Logic.Items.Key;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory, IHeroLocator
    {
        private readonly IAssetProvider _assets;
        private readonly IPersistentProgressService _progressService;
        private readonly IPauseService _pauseService;
        private readonly IUIFactory _uiFactory;
        private readonly IInputService _inputService;
        private readonly ICameraOperatorService _cameraOperator;

        private Transform _heroTransform;

        public GameFactory(
            IAssetProvider assets,
            IPersistentProgressService progressService,
            IPauseService pauseService,
            IUIFactory uiFactory,
            IInputService inputService,
            ICameraOperatorService cameraOperator)
        {
            _assets = assets;
            _progressService = progressService;
            _pauseService = pauseService;
            _uiFactory = uiFactory;
            _inputService = inputService;
            _cameraOperator = cameraOperator;
        }

        Transform IHeroLocator.Location => _heroTransform;

        public void Cleanup() =>
            _progressService.CleanupReadersAndWriters();

        public void WarmUp() =>
            _assets.PreloadCells();

        public GameObject CreateHero(Vector3 at)
        {
            GameObject hero = _assets.InstantiateRegistered(AssetPath.Hero, at);
            _heroTransform = hero.transform;

            return hero;
        }

        public GameObject CreateCell<TCell>(Transform container)
            where TCell : Cell
        {
            GameObject cell = _assets.InstantiateCell<TCell>(container);

            return cell;
        }

        public GameObject CreateLobby() =>
            _assets.InstantiateRegistered(AssetPath.Lobby);

        public GameObject CreateSpireSegment(Vector3 at, Quaternion rotation) =>
            _assets.Instantiate(AssetPath.SpireSegment, at, rotation);

        public GameObject CreateHorizontalRail(Transform container) =>
            _assets.Instantiate(
                AssetPath.HorizontalRail,
                Vector3.zero.ChangeY(container.position.y),
                container.rotation,
                container);

        public GameObject CreateVerticalRail(Transform container) =>
            _assets.Instantiate(AssetPath.VerticalRail, container);

        public GameObject CreateHorizontalRailLock(Transform container) =>
            _assets.Instantiate(
                AssetPath.HorizontalRailLock,
                Vector3.zero.ChangeY(container.position.y),
                container.rotation,
                container);

        public GameObject CreateVerticalRailLock(Transform container) =>
            _assets.Instantiate(AssetPath.VerticalRailLock, container);

        public GameObject CreateTutorialTrigger(Transform container) =>
            _assets.Instantiate(AssetPath.TutorialTrigger, container);

        public IItem CreateItem(StorableStaticData data) =>
            data.ItemType switch
            {
                StorableType.Compass => new Compass(data, _progressService, _uiFactory, this),
                StorableType.Binocular => new Binocular(
                    data,
                    _uiFactory,
                    _inputService,
                    this,
                    _cameraOperator,
                    _progressService),
                StorableType.BlueKey => new Key(data),
                StorableType.RedKey => new Key(data),
                StorableType.GreenKey => new Key(data),
                StorableType.RuneAlpha => new Rune(data),
                StorableType.RuneBeta => new Rune(data),
                StorableType.RuneGamma => new Rune(data),
                StorableType.RuneDelta => new Rune(data),
                StorableType.RuneEpsilon => new Rune(data),
                StorableType.None => throw new Exception("Storable type is not specified"),
                _ => new Item(data),
            };

        public GameObject CreateVirtualMover() =>
            _assets.Instantiate(AssetPath.VirtualMover, _heroTransform.position);

        public Material CreateMaterial(string name) =>
            _assets.LoadAsset<Material>(AssetPath.Combine(AssetPath.Materials, name));

        public PhysicMaterial CreatePhysicMaterial(string name) =>
            _assets.LoadAsset<PhysicMaterial>(AssetPath.Combine(AssetPath.PhysicMaterials, name));

        public GameObject CreateMusicPlayer() =>
            _assets.Instantiate(AssetPath.MusicPlayer);

        public GameObject CreateCamera() =>
            _assets.Instantiate(AssetPath.Camera);

        public GameObject CreateHud() =>
            _assets.InstantiateRegistered(AssetPath.Hud);
    }
}