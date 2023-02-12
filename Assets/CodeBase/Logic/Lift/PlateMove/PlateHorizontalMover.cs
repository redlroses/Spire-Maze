using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    public sealed class PlateHorizontalMoverNew : PlateMover<float>
    {
        protected override void SetNewPosition(float from, float to, float delta)
        {
            float deltaAngle = Mathf.Lerp(from, to, delta);
            RigidBody.rotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
        }

        protected override float GetDistance(LiftDestinationMarker from, LiftDestinationMarker to) =>
            Mathf.Abs(from.Position.Angle - to.Position.Angle);

        protected override float GetTransform(LiftDestinationMarker from)
        {
            UnityEngine.Debug.Log(from.Position.Angle);
            return from.Position.Angle;
        }
    }
}