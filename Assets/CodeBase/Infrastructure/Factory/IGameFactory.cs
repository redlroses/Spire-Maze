using System.Collections.Generic;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Items;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData.Storable;
using CodeBase.Tutorial;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        void Cleanup();
        void WarmUp();
        GameObject CreateHero(Vector3 at);
        GameObject CreateCell<TCell>(Transform container) where TCell : Cell;
        Material CreateMaterial(string name);
        PhysicMaterial CreatePhysicMaterial(string name);
        GameObject CreateHud();
        GameObject CreateLobby();
        IItem CreateItem(StorableStaticData itemType);
        GameObject CreateVirtualMover();
        GameObject CreateSpireSegment(Vector3 at, Quaternion quaternion);
        GameObject CreateHorizontalRail(Transform container);
        GameObject CreateVerticalRail(Transform container);
        GameObject CreateHorizontalRailLock(Transform container);
        GameObject CreateVerticalRailLock(Transform container);
        GameObject CreateTutorialTrigger(Transform container);
        GameObject CreateMusicPlayer();
    }
}