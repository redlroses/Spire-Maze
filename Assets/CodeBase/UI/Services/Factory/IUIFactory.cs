using CodeBase.Services;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace CodeBase.UI.Services.Factory
{
  public interface IUIFactory: IService
  {
    void CreateLeaderboard();
    void CreateUIRoot();
    GameObject CreateExtraLiveView(Transform inside);
    GameObject CreateTopRankView(int rank, Transform inside);
    GameObject CreateRankView(Transform inside);
    void CreateSettings();
    void CreatePause();
    void CreateResults();
    void CreateLose();
  }
}