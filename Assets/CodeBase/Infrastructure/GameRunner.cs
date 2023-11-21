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
            GameBootstrapper bootstrapper = FindObjectOfType<GameBootstrapper>();

            if (bootstrapper == null)
            {
                Instantiate(_bootstrapperPrefab);
            }
#else
            if (SceneManager.GetActiveScene().name.Equals(LevelNames.Initial))
            {
                Instantiate(_bootstrapperPrefab);
            }
#endif
        }
    }
}