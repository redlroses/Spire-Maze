using CodeBase.LevelSpecification;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class CellFactory : MonoBehaviour
    {
        private const string CellPath = "Prefabs/";

        public static GameObject InstantiateCell<TCell>(Transform container) where TCell : Cell =>
            InstantiateObject(container, CellPath + typeof(TCell).Name);

        private static GameObject InstantiateObject(Transform container, string path)
        {
            GameObject prefabPlate = Resources.Load<GameObject>(path);
            return Instantiate(prefabPlate, container);
        }
    }
}