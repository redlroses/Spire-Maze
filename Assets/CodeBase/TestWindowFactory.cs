using NaughtyAttributes;
using UnityEngine;

namespace CodeBase
{
    public class TestWindowFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _windowPrefab;

        [Button("Open")]
        private void Open()
        {
            var gm = Instantiate(_windowPrefab, transform);
            TestWindowWrapper wrapper = gm.GetComponent<TestWindowWrapper>();
            wrapper.Play();
        }

    }
}