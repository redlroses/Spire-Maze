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
                Instantiate(_bootstrapperPrefab);
                return;
            }

            var bootstrapper = FindObjectOfType<GameBootstrapper>();

            if (bootstrapper != null)
            {
                return;
            }

            Instantiate(_bootstrapperPrefab);
        }
    }
}