using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class CellFactory
    {
        public static void InstantiateCell<T>(Transform container)
        {
            Debug.Log($"{container} instantiated");
        }
    }
}