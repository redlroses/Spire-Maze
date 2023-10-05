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
            VirtualMover virtualMover = _gameFactory.CreateVirtualMover().GetComponent<VirtualMover>();
            _inputService.OverviewMove += virtualMover.Move;
            GameObject overviewInterface = _uiFactory.CreateOverviewInterface();
            Button closeButton = overviewInterface.GetComponent<Button>();

            closeButton.onClick.AddListener(() =>
            {
                _inputService.OverviewMove -= virtualMover.Move;
                Object.Destroy(overviewInterface);
                Object.Destroy(virtualMover.gameObject);
                _inputService.DisableOverviewMap();
                _inputService.EnableMovementMap();
            });
        }
    }
}