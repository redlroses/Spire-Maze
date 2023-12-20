using CodeBase.Tools.Constants;
using UnityEngine;

namespace CodeBase.Logic.Lift.PlateMove
{
    public sealed class PlateHorizontalMover : PlateMover<float>
    {
        protected override void UpdatePosition(float from, float to, float delta)
        {
            float deltaAngle = Mathf.Lerp(from, to, delta);
            Rigidbody.MovePosition(GetPosition(deltaAngle, Radius));
            Rigidbody.MoveRotation(Quaternion.AngleAxis(deltaAngle, Vector3.up));
        }

        protected override float GetDistance(LiftDestinationMarker from, LiftDestinationMarker to) =>
            Mathf.PI * Radius * Mathf.Abs(from.Position.Angle - to.Position.Angle) / Trigonometry.PiGrade;

        protected override float GetTransform(LiftDestinationMarker from) =>
            from.Position.Angle;

        private Vector3 GetPosition(float byArcGrade, float radius)
        {
            float posX = Mathf.Cos(-byArcGrade * Mathf.Deg2Rad) * radius;
            float posZ = Mathf.Sin(-byArcGrade * Mathf.Deg2Rad) * radius;

            return new Vector3(posX, Position.y, posZ);
        }
    }
}