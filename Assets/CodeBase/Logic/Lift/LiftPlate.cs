using CodeBase.Logic.Lift.PlateMove;
using CodeBase.Tools.Constants;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public class LiftPlate : MonoCache, ILiftPlate
    {
        [SerializeField] private LiftState _state;

        private IPlateMover _plateMover;
        private LiftDestinationMarker _currentMarker;
        private LiftDestinationMarker _destinationMarker;
        private float _timer;
        private bool _isTriggered;
        public LiftState State => _state;

        // protected override void Run()
        // {
        //     if (_state == LiftState.Moving)
        //     {
        //         return;
        //     }
        //
        //     if (_isTriggered)
        //     {
        //         _timer -= Time.deltaTime;
        //     }
        //
        //     if (_timer > 0)
        //     {
        //         return;
        //     }
        //
        //     Move();
        // }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPlateMovable plateMovable) == false)
            {
                return;
            }

            Debug.Log("TriggerEnter");
            plateMovable.OnMovingPlatformEnter(_plateMover);
            _timer = ConstantsGeneral.DelayBeforeRelocation;
            _isTriggered = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IPlateMovable plateMovable) == false)
            {
                return;
            }

            Debug.Log("TriggerExit");
            plateMovable.OnMovingPlatformExit(_plateMover);
            _isTriggered = false;
        }

        public void Construct(LiftDestinationMarker initialMarker, LiftDestinationMarker destinationMarker, IPlateMover mover)
        {
            _currentMarker = initialMarker;
            _destinationMarker = destinationMarker;
            _plateMover = mover;
        }

        [ContextMenu("Move")]
        public void Move()
        {
            Debug.Log("Move");
            _plateMover.Move(_currentMarker, _destinationMarker);
            _state = LiftState.Moving;
            SwitchMarkers();
        }

        private void SwitchMarkers()
        {
            LiftDestinationMarker temp = _currentMarker;
            _currentMarker = _destinationMarker;
            _destinationMarker = temp;
        }
    }
}
