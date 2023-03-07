using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.Portal
{
    [RequireComponent(typeof(TimerOperator))]
    public class PortalGate : ObserverTargetExited<TeleportableObserver, ITeleportable>
    {
        [SerializeField] private float _waitDelay;
        [SerializeField] private PortalGate _linkedPortalGate;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private TimerOperator _timer;

        private ITeleportable _teleportable;
        private bool _isRecipient;
        private Transform _selfTransform;

        public void Construct(PortalGate linked)
        {
            _linkedPortalGate = linked;
            _selfTransform = transform;
            _timer ??= Get<TimerOperator>();
            _timer.SetUp(_waitDelay, Teleport);
        }

        protected override void OnTriggerObserverEntered(ITeleportable teleporter)
        {
            if (_isRecipient)
            {
                return;
            }

            _timer.Restart();
            _timer.Play();
            _teleportable = teleporter;
        }

        protected override void OnTriggerObserverExited(ITeleportable teleporter)
        {
            _timer.Pause();
            _isRecipient = false;
        }

        private void Receive(ITeleportable teleportable, float dotRotation)
        {
            _effect.Play();
            _isRecipient = true;
            Vector3 forward = _selfTransform.forward;
            Vector3 rotation = dotRotation > 0 ? forward : forward * -1;
            teleportable.Teleportation(_selfTransform.position, rotation);
        }

        private void Teleport()
        {
            float dotRotation = Vector3.Dot(_selfTransform.forward, _teleportable.Forward);
            _linkedPortalGate.Receive(_teleportable, dotRotation);
            _effect.Play();
        }
    }
}