using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private GameBootstrapper _bootstrapperPrefab;

        private void Awake()
        {
            if (Application.isEditor == false)
            {
                Debug.Log("if (Application.isEditor == false)");
                Instantiate(_bootstrapperPrefab);
                return;
            }

            var bootstrapper = FindObjectOfType<GameBootstrapper>();

            if (bootstrapper != null)
            {
                Debug.Log("if (bootstrapper != null)");
                return;
            }

            Debug.Log("  Instantiate(_bootstrapperPrefab);");
            Instantiate(_bootstrapperPrefab);
        }
    }
}