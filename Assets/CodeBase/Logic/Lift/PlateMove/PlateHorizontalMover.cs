using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    public sealed class PlateHorizontalMover : PlateMover<float>
    {
        protected override void SetNewPosition(float from, float to, float delta)
        {
            float deltaAngle = Mathf.Lerp(from, to, delta);
            RigidBody.rotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
            RigidBody.MovePosition(GetPosition(deltaAngle, Radius));
        }

        protected override float GetDistance(LiftDestinationMarker from, LiftDestinationMarker to) =>
            Mathf.Abs(from.Position.Angle - to.Position.Angle);

        protected override float GetTransform(LiftDestinationMarker from)
        {
            Debug.Log(from.Position.Angle);
            return from.Position.Angle;
        }

        private Vector3 GetPosition(float byArcGrade, float radius)
        {
            float posX = Mathf.Cos(-byArcGrade * Mathf.Deg2Rad) * radius;
            float posZ = Mathf.Sin(-byArcGrade * Mathf.Deg2Rad) * radius;
            return new Vector3(posX, RigidBody.position.y, posZ);
        }
    }
}