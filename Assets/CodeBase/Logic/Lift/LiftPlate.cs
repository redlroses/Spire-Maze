using CodeBase.Logic.Lift.PlateMove;
using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.Lift
{
    [RequireComponent(typeof(TimerOperator))]
    [RequireComponent(typeof(PlateMovableObserver))]
    public class LiftPlate : ObserverTargetExited<PlateMovableObserver, IPlateMovable>, ILiftPlate
    {
        [SerializeField] private LiftState _state;
        [SerializeField] private float _waitDelay;
        [SerializeField] private TimerOperator _timer;

        private IPlateMover _plateMover;
        private LiftDestinationMarker _currentMarker;
        private LiftDestinationMarker _destinationMarker;
        public LiftState State => _state;

        public void Construct(LiftDestinationMarker initialMarker, LiftDestinationMarker destinationMarker, IPlateMover mover)
        {
            _timer ??= Get<TimerOperator>();
            _currentMarker = initialMarker;
            _destinationMarker = destinationMarker;
            _currentMarker.Call += OnCall;
            _destinationMarker.Call += OnCall;
            _plateMover = mover;
            _plateMover.MoveEnded += OnMoveEnded;
            _timer.SetUp(_waitDelay, Move);
        }

        private void OnCall(LiftDestinationMarker caller)
        {
            if (_currentMarker != caller)
            {
                Move();
            }
        }

        private void OnDestroy()
        {
            _plateMover.MoveEnded -= OnMoveEnded;
            _currentMarker.Call -= OnCall;
            _destinationMarker.Call -= OnCall;
        }

        [ContextMenu("Move")]
        public void Move()
        {
            if (_state == LiftState.Moving)
            {
                return;
            }

            _plateMover.Move(_currentMarker, _destinationMarker);
            _state = LiftState.Moving;
            SwitchMarkers();
        }

        protected override void OnTriggerObserverEntered(IPlateMovable plateMovable)
        {
            plateMovable.OnMovingPlatformEnter(_plateMover);

            if (_state == LiftState.Moving)
            {
                return;
            }

            _timer.Restart();
            _timer.Play();
        }

        protected override void OnTriggerObserverExited(IPlateMovable plateMovable)
        {
            plateMovable.OnMovingPlatformExit(_plateMover);
            _timer.Pause();
        }

        private void OnMoveEnded()
        {
            _state = LiftState.Idle;
        }

        private void SwitchMarkers()
        {
            LiftDestinationMarker temp = _currentMarker;
            _currentMarker = _destinationMarker;
            _destinationMarker = temp;
        }
    }
}
