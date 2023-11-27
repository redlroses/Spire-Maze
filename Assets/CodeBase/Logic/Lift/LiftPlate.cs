using CodeBase.EditorCells;
using CodeBase.Logic.Lift.PlateMove;
using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Logic.Lift
{
    [RequireComponent(typeof(TimerOperator))]
    [RequireComponent(typeof(PlateMovableObserver))]
    public class LiftPlate : ObserverTargetExited<PlateMovableObserver, IMovableByPlate>, ILiftPlate
    {
        [SerializeField] private LiftState _state;
        [SerializeField] private float _waitDelay;
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private LiftAnimator _liftAnimator;

        private IPlateMover _plateMover;
        private LiftDestinationMarker _currentMarker;
        private LiftDestinationMarker _destinationMarker;
        public LiftState State => _state;

        public void Construct(LiftDestinationMarker initialMarker, LiftDestinationMarker destinationMarker, IPlateMover mover, PlateMoveDirection initialDirection)
        {
            _timer ??= GetComponent<TimerOperator>();
            _timer.SetUp(_waitDelay, Move);
            _currentMarker = initialMarker;
            _destinationMarker = destinationMarker;
            _currentMarker.Called += OnCalled;
            _destinationMarker.Called += OnCalled;
            _plateMover = mover;
            _plateMover.MoveEnded += OnMoveEnded;
            _liftAnimator.Construct(initialDirection, mover);
        }

        private void OnDestroy()
        {
            _plateMover.MoveEnded -= OnMoveEnded;
            _currentMarker.Called -= OnCalled;
            _destinationMarker.Called -= OnCalled;
        }

        protected override void OnTriggerObserverEntered(IMovableByPlate movableByPlate)
        {
            movableByPlate.OnMovingPlatformEnter(_plateMover);

            if (_state == LiftState.Moving)
            {
                return;
            }

            _timer.Restart();
            _timer.Play();
        }

        protected override void OnTriggerObserverExited(IMovableByPlate movableByPlate)
        {
            movableByPlate.OnMovingPlatformExit(_plateMover);
            _timer.Pause();
        }

        [ContextMenu("Move")]
        public void Move()
        {
            if (_state == LiftState.Moving)
            {
                return;
            }

            _liftAnimator.StartAnimation();
            _plateMover.Move(_currentMarker, _destinationMarker);
            _state = LiftState.Moving;
            SwitchMarkers();
        }

        private void OnCalled(LiftDestinationMarker caller)
        {
            if (_currentMarker != caller)
            {
                Move();
            }
        }

        private void OnMoveEnded()
        {
            _state = LiftState.Idle;
            _liftAnimator.StopAnimation();
        }

        private void SwitchMarkers()
        {
            (_currentMarker, _destinationMarker) = (_destinationMarker, _currentMarker);
        }
    }
}
