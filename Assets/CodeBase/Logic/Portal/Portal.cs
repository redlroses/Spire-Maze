using UnityEngine;
using NTC.Global.Cache;
using CodeBase.Tools.Constants;

namespace CodeBase.Logic.Portal
{
    public class Portal : MonoCache
    {
        [SerializeField] private Portal _linkedPortal;
        [SerializeField] private ParticleSystem _effect;

        private Teleporter _enteredObject;
        private float _timer;
        private bool _isTriggered;
        private bool _isRecipient;


        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private Color _color;


        protected override void Run()
        {
            if (_enteredObject == null)
            {
                return;
            }

            if (_isTriggered)
            {
                _timer -= Time.deltaTime;
            }

            if (_timer <= 0.35f)
            {
                _effect.Play();
            }

            if (_timer <= 0)
            {
                _effect.Play();
                _enteredObject.Teleport(_linkedPortal);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isRecipient)
                return;

            if (other.TryGetComponent(out Teleporter teleporter) == false)
            {
                return;
            }

            _timer = ConstantsGeneral.DelayBeforeRelocation;
            _isTriggered = true;
            _enteredObject = teleporter;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Teleporter _) == false)
            {
                return;
            }

            _isTriggered = false;
            _enteredObject = null;
            _isRecipient = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawCube(transform.position, _boxCollider.size);
        }

        public void SetRecipet() => _isRecipient = true;
    }
}