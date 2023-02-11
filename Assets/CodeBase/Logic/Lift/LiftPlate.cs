using CodeBase.Logic.Lift.PlateMove;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public class LiftPlate : MonoCache, ILiftPlate
    {
        [SerializeField] private LiftState _state;
        [SerializeField] private LiftDestinationMarker _defaultInitialMarker;
        [SerializeField] private LiftDestinationMarker _defaultDestinationMarker;

        private IPlateMover _plateHorizontalMover;
        private LiftDestinationMarker _currentMarker;
        private LiftDestinationMarker _destinationMarker;

        public LiftState State => _state;

        private void Awake()
        {
            _plateHorizontalMover = Get<IPlateMover>();
            _currentMarker = _defaultInitialMarker;
            _destinationMarker = _defaultDestinationMarker;
        }

        [ContextMenu("Move")]
        public void Move()
        {
            _plateHorizontalMover.Move(_currentMarker, _destinationMarker);
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