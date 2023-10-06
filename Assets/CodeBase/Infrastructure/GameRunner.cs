using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private GameBootstrapper _bootstrapperPrefab;

        private void Awake()
        {
#if UNITY_EDITOR
            var bootstrapper = FindObjectOfType<GameBootstrapper>();

            if (bootstrapper == null)
            {
                Instantiate(_bootstrapperPrefab);
            }
#else
            if (SceneManager.GetActiveScene().name.Equals(LevelNames.Initial))
            {
                Debug.Log($"Instantiate scene name {SceneManager.GetActiveScene().name}");
                Instantiate(_bootstrapperPrefab);
            }
#endif
        }
    }
}