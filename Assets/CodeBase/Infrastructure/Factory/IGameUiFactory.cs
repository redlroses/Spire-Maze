using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameUiFactory : IService
    {
        GameObject CreateHud();
        GameObject CreateExtraLiveView(Transform inside);
        GameObject CreateTopRankView(int rank, Transform inside);
        GameObject CreateRankView(Transform inside);
    }
}