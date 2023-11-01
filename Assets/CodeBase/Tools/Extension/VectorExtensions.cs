using System.ComponentModel;
using CodeBase.Tools.Constants;
using CodeBase.Tools.PhysicsDebug;
using UnityEngine;

namespace CodeBase.Tools.Extension
{
    public static class VectorExtensions
    {
        public static Vector3 AddY(this Vector2 vector, float y) =>
            new Vector3(vector.x, y, vector.y);

        public static Vector2 RemoveY(this Vector3 vector) =>
            new Vector2(vector.x, vector.z);

        public static Vector3 ChangeY(this Vector3 vector, float to)
        {
            vector.y = to;
            return vector;
        }

        public static Vector3 ChangeX(this Vector3 vector, float to)
        {
            vector.x = to;
            return vector;
        }

        public static Vector3 ChangeZ(this Vector3 vector, float to)
        {
            vector.z = to;
            return vector;
        }

        public static float SqrMagnitudeTo(this Vector3 from, Vector3 to) =>
            Vector3.SqrMagnitude(to - from);

        public static Vector3 ToWorldPosition(this Vector2 vector, float radius)
        {
            float arcLength = 2f * Mathf.PI * radius;
            float clampedX = vector.x.ClampRound(0, arcLength);
            float lerpedX = Mathf.Lerp(0, Trigonometry.TwoPiGrade, clampedX / arcLength);
            float posX = Mathf.Cos(lerpedX * Mathf.Deg2Rad) * radius;
            float posZ = Mathf.Sin(lerpedX * Mathf.Deg2Rad) * radius;
            return new Vector3(posX, vector.y, posZ);
        }

        public static Vector2 Rotate(this Vector2 vector, float byAngle)
        {
            float rotatedX = vector.x * Mathf.Cos(byAngle) - vector.y * Mathf.Sin(byAngle);
            float rotatedY = vector.x * Mathf.Sin(byAngle) + vector.y * Mathf.Cos(byAngle);
            return new Vector2(rotatedX, rotatedY);
        }

        public static Vector3 ToWorldDirection(this Vector2 vector, Vector3 position, float radius)
        {
            float deltaAngle = Mathf.Asin(vector.x * Arithmetic.Half / radius) / Arithmetic.Half;
            Vector2 currentFlatPosition = position.RemoveY();
            Vector2 rotatedPosition = currentFlatPosition.Rotate(deltaAngle);
            rotatedPosition = rotatedPosition.normalized * radius;
            Vector2 direction = rotatedPosition - currentFlatPosition;
            return new Vector3(direction.x, vector.y, direction.y);
        }
    }
}