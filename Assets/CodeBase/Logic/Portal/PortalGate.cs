using CodeBase.Data;
using CodeBase.Data.CellStates;
using CodeBase.Logic.Observer;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic.Portal
{
    [RequireComponent(typeof(TimerOperator))]
    public class PortalGate : ObserverTargetExited<TeleportableObserver, ITeleportable>, ISavedProgress, IIndexable
    {
        [SerializeField] private float _waitDelay;
        [SerializeField] private PortalGate _linkedPortalGate;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private TimerOperator _timer;

        private ITeleportable _teleportable;
        private bool _isRecipient;
        private Transform _selfTransform;

        public int Id { get; private set; }
        public bool IsActivated { get; private set; }

        public void Construct(int id, PortalGate linked)
        {
            Id = id;
            _linkedPortalGate = linked;
            _selfTransform = transform;
            _timer ??= Get<TimerOperator>();
            _timer.SetUp(_waitDelay, Teleport);
        }

        protected override void OnTriggerObserverEntered(ITeleportable teleporter)
        {
            if (_isRecipient || IsActivated == false)
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

            if (cellState == null || cellState.IsActivated == false)
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

        private void Receive(ITeleportable teleportable, float dotRotation)
        {
            _effect.Play();
            _isRecipient = true;
            Vector3 forward = _selfTransform.forward;
            Vector3 rotation = dotRotation > 0 ? forward : forward * -1;
            teleportable.Teleportation(_selfTransform.position, rotation);
        }

        private void Activate() =>
            IsActivated = true;

        private void Teleport()
        {
            float dotRotation = Vector3.Dot(_selfTransform.forward, _teleportable.Forward);
            _linkedPortalGate.Activate();
            _linkedPortalGate.Receive(_teleportable, dotRotation);
            _effect.Play();
        }
    }
}