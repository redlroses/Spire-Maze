using System.Collections;
using System.Collections.Generic;
using CodeBase.EditorCells;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Services;
using CodeBase.Services.Pause;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Score;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    void Cleanup();
    void WarmUp();
    Material CreateColoredMaterial(Colors color);
    GameObject CreateSpire();
    void InitScoreService();
    GameObject CreateHero(Vector3 at);
    GameObject CreateEnemy(string prefabPath, Vector3 position);
    GameObject CreateCell<TCell>(Transform container) where TCell : Cell;
    Material CreateMaterial(string name);
    PhysicMaterial CreatePhysicMaterial(string name);
    GameObject CreateHud();
    GameObject CreateLobby();
  }
}