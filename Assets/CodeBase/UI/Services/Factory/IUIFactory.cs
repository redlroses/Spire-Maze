﻿using CodeBase.Services;
using CodeBase.UI.Elements;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace CodeBase.UI.Services.Factory
{
  public interface IUIFactory : IService
  {
    void CreateLeaderboard();
    void CreateUIRoot();
    GameObject CreateExtraLiveView(Transform inside);
    GameObject CreateTopRankView(int rank, Transform inside);
    GameObject CreateRankView(Transform inside);
    GameObject CreateCellView(Transform selfTransform);
    void CreateSettings();
    void CreatePause();
    void CreateResults();
    void CreateLose();
  }
}