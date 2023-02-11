using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    public sealed class PlateVerticalMover : PlateMover<Vector3>
    {
        protected override void SetNewPosition(Vector3 from, Vector3 to, float delta) =>
            RigidBody.position = Vector3.Lerp(from, to, delta);

        protected override float GetDistance(LiftDestinationMarker @from, LiftDestinationMarker to) =>
            Mathf.Abs(from.Position.Height - to.Position.Height);

        protected override Vector3 GetTransform(LiftDestinationMarker from) =>
            new Vector3(0, from.Position.Height, 0);
    }
}