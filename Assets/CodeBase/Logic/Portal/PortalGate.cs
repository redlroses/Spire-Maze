using CodeBase.Data;
using CodeBase.Data.CellStates;
using CodeBase.Logic.Observer;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic.Portal
{
    [RequireComponent(typeof(TimerOperator))]
    public class PortalGate : ObserverTargetExited<TeleportableObserver, ITeleportable>, ISavedProgress
    {
        [SerializeField] private float _waitDelay;
        [SerializeField] private PortalGate _linkedPortalGate;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private TimerOperator _timer;

        private int _id;
        private ITeleportable _teleportable;
        private bool _isRecipient;
        private Transform _selfTransform;
        private bool _isActivated;

        public void Construct(int id, PortalGate linked)
        {
            _id = id;
            _linkedPortalGate = linked;
            _selfTransform = transform;
            _timer ??= Get<TimerOperator>();
            _timer.SetUp(_waitDelay, Teleport);
        }

        protected override void OnTriggerObserverEntered(ITeleportable teleporter)
        {
            if (_isRecipient || _isActivated == false)
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

        public void LoadProgress(PlayerProgress progress)
        {
            PortalState cellState = progress.WorldData.LevelState.PortalStates
                .Find(cell => cell.Id == _id);

            if (cellState == null || cellState.IsActivated == false)
            {
                return;
            }

            _isActivated = cellState.IsActivated;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            PortalState cellState = progress.WorldData.LevelState.PortalStates
                .Find(cell => cell.Id == _id);

            if (cellState == null)
            {
                progress.WorldData.LevelState.PortalStates.Add(new PortalState(_id, _isActivated));
            }
            else
            {
                cellState.IsActivated = _isActivated;
            }
        }

        private void Receive(ITeleportable teleportable, float dotRotation)
        {
            _effect.Play();
            _isRecipient = true;
            Vector3 forward = _selfTransform.forward;
            Vector3 rotation = dotRotation > 0 ? forward : forward * -1;
            teleportable.Teleportation(_selfTransform.position, rotation);
        }

        private void Activate() =>
            _isActivated = true;

        private void Teleport()
        {
            float dotRotation = Vector3.Dot(_selfTransform.forward, _teleportable.Forward);
            _linkedPortalGate.Activate();
            _linkedPortalGate.Receive(_teleportable, dotRotation);
            _effect.Play();
        }
    }
}