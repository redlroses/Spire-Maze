using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Storable;
using CodeBase.Tools.Extension;
using CodeBase.UI;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.Logic.Items
{
    public sealed class Compass : Item, IUsable
    {
        private readonly IHeroLocator _locator;
        private readonly IPersistentProgressService _progressService;
        private readonly IUIFactory _uiFactory;

        private CompassArrowPanel _arrow;

        public Compass(
            StorableStaticData storableData,
            IPersistentProgressService progressService,
            IUIFactory uiFactory,
            IHeroLocator locator)
            : base(storableData)
        {
            _progressService = progressService;
            _locator = locator;
            _uiFactory = uiFactory;
        }

        public void Use()
        {
            if (_arrow is null)
            {
                _arrow = CreateArrow();
            }
            else
            {
                _arrow.Destroy();
                _arrow = null;
            }
        }

        private CompassArrowPanel CreateArrow()
        {
            GameObject panel = _uiFactory.CreateCompassArrowPanel();

            CompassArrowPanel compassPanel = panel.GetComponent<CompassArrowPanel>();
            Canvas canvas = panel.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;

            compassPanel.Construct(
                _progressService.Progress.WorldData.LevelPositions.FinishPosition.AsUnityVector(),
                _locator.Location);

            return compassPanel;
        }
    }
}