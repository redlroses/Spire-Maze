using System;
using CodeBase.Data;
using CodeBase.Logic.Observer;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic.Portal
{
    [RequireComponent(typeof(TimerOperator))]
    public class PortalGate : ObserverTargetExited<TeleportableObserver, ITeleportable>, ISavedProgress, IIndexable
    {
        [SerializeField] private float _waitDelay;
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private PortalGateEffector _effector;

        private PortalGate _linkedPortalGate;
        private ITeleportable _teleportable;
        private Transform _selfTransform;
        private bool _isRecipient;

        public event Action Teleported = () => { };

        public int Id { get; private set; }

        public bool IsActivated { get; private set; }

        public void Construct(int id, PortalGate linked, Color32 color)
        {
            Id = id;
            _linkedPortalGate = linked;
            _selfTransform = transform;
            _effector.Construct(color);

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

        public void LoadProgress(PlayerProgress progress)
        {
            IndexableState cellState = progress.WorldData.LevelState.Indexables
                .Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                return;
            }

            IsActivated = cellState.IsActivated;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            IndexableState cellState = progress.WorldData.LevelState.Indexables
                .Find(cell => cell.Id == Id);

            if (cellState == null)
            {
                progress.WorldData.LevelState.Indexables.Add(new IndexableState(Id, IsActivated));
            }
            else
            {
                cellState.IsActivated = IsActivated;
            }
        }

        private void Activate() =>
            IsActivated = true;

        private void Receive(ITeleportable teleportable, float dotRotation)
        {
            Teleported.Invoke();
            _isRecipient = true;
            Vector3 forward = _selfTransform.forward;
            Vector3 rotation = dotRotation > 0 ? forward : -forward;
            teleportable.Teleportation(_selfTransform.position, rotation);
        }

        private void Teleport()
        {
            float dotRotation = Vector3.Dot(_selfTransform.forward, _teleportable.Forward);
            _linkedPortalGate.Activate();
            _linkedPortalGate.Receive(_teleportable, dotRotation);
            Teleported.Invoke();
        }
    }
}