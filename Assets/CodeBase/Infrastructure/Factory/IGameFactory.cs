using System.Collections.Generic;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    void Cleanup();
  }
}