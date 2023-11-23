﻿using CodeBase.StaticData.Storable;
using CodeBase.UI.Services.Factory;

namespace CodeBase.Logic.Items
{
    public sealed class Compass : Item, IUsable, IReloadable
    {
        private readonly IUIFactory _uiFactory;
        private readonly IHeroLocator _locator;

        public float ReloadTime { get; }

        public Compass(StorableStaticData staticData, IUIFactory uiFactory, IHeroLocator locator) : base(staticData)
        {
            _locator = locator;
            _uiFactory = uiFactory;
            ReloadTime = staticData.ReloadTime;
        }

        public void Use()
        {
            _uiFactory.CreateCompassArrowPanel(_locator.Location, ReloadTime);
        }
    }
}