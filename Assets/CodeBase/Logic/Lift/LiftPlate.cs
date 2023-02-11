using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public class LiftPlate : MonoCache, ILiftPlate
    {
        [SerializeField] private LiftState _state;
        [SerializeField] private LiftDestinationMarker _defaultInitialMarker;
        [SerializeField] private LiftDestinationMarker _defaultDestinationMarker;

        private PlateHorizontalMover _plateHorizontalMover;
        private LiftDestinationMarker _currentMarker;
        private LiftDestinationMarker _destinationMarker;

        public LiftState State => _state;

        private void Awake()
        {
            _plateHorizontalMover = Get<PlateHorizontalMover>();
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