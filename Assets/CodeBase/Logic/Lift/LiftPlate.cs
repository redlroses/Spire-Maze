using CodeBase.Logic.Lift.PlateMove;
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

        public LiftState State => _state;

        public void Construct(LiftDestinationMarker initialMarker, LiftDestinationMarker destinationMarker, IPlateMover mover)
        {
            _currentMarker = initialMarker;
            _destinationMarker = destinationMarker;
            _plateMover = mover;
        }

        [ContextMenu("Move")]
        public void Move()
        {
            _plateMover.Move(_currentMarker, _destinationMarker);
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