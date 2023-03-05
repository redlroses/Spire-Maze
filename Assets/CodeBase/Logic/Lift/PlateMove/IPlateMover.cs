using System;
using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    public interface IPlateMover
    {
        void Move(LiftDestinationMarker from, LiftDestinationMarker to);
        event Action<Vector3, Vector3> PositionUpdated;
        event Action MoveEnded;
    }
}