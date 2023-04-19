using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class ExtraLiveView : MonoBehaviour
    {
        [SerializeField] private Image _heart;
        [SerializeField] private bool _isActive;

        public bool IsActive => _isActive;

        public void Activate()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;
            _heart.enabled = true;
        }

        public void Deactivate()
        {
            if (_isActive == false)
            {
                return;
            }

            _isActive = false;
            _heart.enabled = false;
        }
    }
}