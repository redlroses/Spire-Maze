using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameUiFactory : IGameUiFactory
    {
        private readonly IAssetProvider _assetProvider;

        public GameUiFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public GameObject CreateExtraLiveView(Transform inside) =>
            _assetProvider.Instantiate(AssetPath.ExtraLiveView, inside);

        public GameObject CreateHud() =>
            _assetProvider.Instantiate(AssetPath.HudPath);
    }
}