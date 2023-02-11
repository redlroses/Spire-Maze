using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    public sealed class PlateHorizontalMover : PlateMover<Quaternion>
    {
        protected override void SetNewPosition(Quaternion from, Quaternion to, float delta) =>
            RigidBody.rotation = Quaternion.Lerp(from, to, delta);

        protected override Quaternion GetTransform(LiftDestinationMarker from) =>
            Quaternion.Euler(0, from.Position.Angle, 0);
    }
}