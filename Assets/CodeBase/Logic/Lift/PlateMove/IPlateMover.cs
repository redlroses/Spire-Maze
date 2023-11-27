using System;
using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    public interface IPlateMover
    {
        event Action<Vector3, Vector3> PositionUpdated;
        event Action MoveEnded;
        float Velocity { get; }
        void Move(LiftDestinationMarker from, LiftDestinationMarker to);
    }
}