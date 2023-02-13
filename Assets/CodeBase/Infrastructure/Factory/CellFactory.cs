using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class CellFactory : MonoBehaviour
    {
        public static GameObject InstantiatePlate(Transform container)
        {
            GameObject prefabPlate = Resources.Load<GameObject>("Prefabs/ArcPlate");
            Debug.Log($"{prefabPlate} inst");
            return Instantiate(prefabPlate, container);
        }

        public static GameObject InstantiateWall(Transform container)
        {
            GameObject prefabWall = Resources.Load<GameObject>("Prefabs/ArcWall");
            Debug.Log($"{prefabWall} inst");
            return Instantiate(prefabWall, container);
        }
    }
}