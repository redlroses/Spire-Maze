using System.Collections.Generic;
using CodeBase.Data.Cell;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    public void Cleanup();
    public Material CreateColoredMaterial(Colors color);
  }
}