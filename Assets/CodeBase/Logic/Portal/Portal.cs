using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.Portal
{
    [RequireComponent(typeof(TimerOperator))]
    public class Portal : ObserverTargetExited<TeleportableObserver, ITeleportable>
    {
        [SerializeField] private float _waitDelay;
        [SerializeField] private Portal _linkedPortal;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private TimerOperator _timer;

        private ITeleportable _teleportable;
        private bool _isRecipient;
        private Transform _selfTransform;

        private void Awake()
        {
            Constructor(_linkedPortal);
        }

        public void Constructor(Portal linked)
        {
            _linkedPortal = linked;
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

        private void Receive(ITeleportable teleportable)
        {
            _effect.Play();
            _isRecipient = true;
            teleportable.Teleportation(_selfTransform.position);
        }

        private void Teleport()
        {
            _linkedPortal.Receive(_teleportable);
            _effect.Play();
        }
    }
}