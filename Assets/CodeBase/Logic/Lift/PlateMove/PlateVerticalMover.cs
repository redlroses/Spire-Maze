﻿using CodeBase.Services.Pause;
using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    public sealed class PlateVerticalMover : PlateMover<Vector3>, IPauseWatcher
    {
        protected override void SetNewPosition(Vector3 from, Vector3 to, float delta) => 
            RigidBody.position = Vector3.Lerp(from, to, delta);

        protected override float GetDistance(LiftDestinationMarker from, LiftDestinationMarker to) =>
            Mathf.Abs(from.Position.Height - to.Position.Height);

        protected override Vector3 GetTransform(LiftDestinationMarker from)
        {
            float posX = Mathf.Cos(-from.Position.Angle * Mathf.Deg2Rad) * Radius;
            float posZ = Mathf.Sin(-from.Position.Angle * Mathf.Deg2Rad) * Radius;
            return new Vector3(posX, from.Position.Height, posZ);
        }
    }
}