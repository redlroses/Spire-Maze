using CodeBase.LevelSpecification.Cells;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
  public interface IAssetProvider : IService
  {
    void LoadCells();
    void Cleanup();
    GameObject Instantiate(string path);
    GameObject Instantiate(string path, Vector3 at);
    GameObject Instantiate(string path, Transform inside);
    GameObject Instantiate(string path, Vector3 at, Transform inside);
    GameObject Instantiate(string path, Vector3 at, Quaternion rotation);
    GameObject Instantiate(string path, Vector3 at, Quaternion rotation, Transform inside);
    GameObject InstantiateCell<TCell>(Transform container) where TCell : Cell;
    TType LoadAsset<TType>(string path) where TType : Object;
  }
}