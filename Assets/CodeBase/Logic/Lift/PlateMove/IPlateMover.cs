using System;
using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    public interface IPlateMover
    {
        void Move(LiftDestinationMarker from, LiftDestinationMarker to);
        Vector3 DeltaPosition { get; }
        Vector3 DeltaRotation { get; }
        event Action<Vector3, Vector3> PositionUpdated;
    }
}