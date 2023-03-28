using CodeBase.LevelSpecification.Cells;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
  public interface IAssetProvider : IService
  {
    void LoadCells();
    void CleanUp();
    GameObject Instantiate(string path, Vector3 at);
    GameObject Instantiate(string path, Transform inside);
    GameObject Instantiate(string path);
    GameObject InstantiateCell<TCell>(Transform container) where TCell : Cell;
  }
}