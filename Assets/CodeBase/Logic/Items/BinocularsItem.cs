using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.Movement;
using CodeBase.Services.Input;
using CodeBase.StaticData.Storable;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Logic.Items
{
    public class BinocularsItem : Item, IUsable, IReloadable
    {
        private readonly IUIFactory _uiFactory;
        private readonly IPlayerInputService _inputService;
        private readonly IGameFactory _gameFactory;
        private VirtualMover _virtualMover;
        public float ReloadTime { get; }

        public BinocularsItem(StorableStaticData staticData, IUIFactory uiFactory, IPlayerInputService inputService, IGameFactory gameFactory) : base(staticData)
        {
            _uiFactory = uiFactory;
            _inputService = inputService;
            _gameFactory = gameFactory;
            ReloadTime = staticData.ReloadTime;
        }

        public void Use()
        {
            _inputService.DisableMovementMap();
            _inputService.EnableOverviewMap();
            _virtualMover = _gameFactory.CreateVirtualMover().GetComponent<VirtualMover>();
            _inputService.OverviewMove += OnOverviewMove;
            GameObject overviewInterface = _uiFactory.CreateOverviewInterface();
            Button closeButton = overviewInterface.GetComponent<Button>();

            closeButton.onClick.AddListener(() =>
            {
                _inputService.OverviewMove -= OnOverviewMove;
                Object.Destroy(overviewInterface);
                Object.Destroy(_virtualMover.gameObject);
                _inputService.DisableOverviewMap();
                _inputService.EnableMovementMap();
            });
        }

        private void OnOverviewMove(Vector2 direction) =>
            _virtualMover.Move(direction);
    }
}