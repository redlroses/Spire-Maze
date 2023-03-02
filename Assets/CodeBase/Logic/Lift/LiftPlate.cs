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
        private Player _enteredObject;
        private float _timer;
        private bool _isTriggered;

        public LiftState State => _state;

        protected override void Run()
        {
            if (_enteredObject == null)
            {
                return;
            }

            if (_state == LiftState.Moving)
            {
                return;
            }

            if (_isTriggered)
            {
                _timer -= Time.deltaTime;
            }

            if (_timer > 0)
            {
                return;
            }
           
            Move();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player) == false)
            {
                return;
            }

            Debug.Log("TriggerEnter");
            _timer = ConstantsGeneral.DelayBeforeRelocation;
            _isTriggered = true;
            _enteredObject = player;
            _enteredObject.transform.SetParent(transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player _) == false)
            {
                return;
            }

            Debug.Log("TriggerExit");
            _isTriggered = false;
            _enteredObject.transform.SetParent(null);
            _enteredObject = null;
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