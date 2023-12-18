using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        private void Awake()
        {
            if (_closeButton != null)
                _closeButton.onClick.AddListener(() => Destroy(gameObject));

            OnAwake();
        }

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() =>
            Cleanup();

        protected virtual void OnAwake()
        {
        }

        protected virtual void Initialize()
        {
        }

        protected virtual void SubscribeUpdates()
        {
        }

        protected virtual void Cleanup()
        {
        }

        protected void Close()
        {
            if (_closeButton != null)
                _closeButton.onClick.Invoke();
            else
                Destroy(gameObject);
        }
    }
}