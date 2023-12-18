using CodeBase.Tools.Constants;
using UnityEngine;

namespace CodeBase.Tools.Extension
{
    public static class AngleExtension
    {
        public static float Clamp360(this float angle)
        {
            angle %= Trigonometry.TwoPiGrade;

            if (angle < 0)
            {
                angle += Trigonometry.TwoPiGrade;
            }

            return angle;
        }

        public static Vector3 ClampY360(this Vector3 rotation)
        {
            float angle = rotation.y % Trigonometry.TwoPiGrade;

            if (angle < 0)
            {
                angle += Trigonometry.TwoPiGrade;
            }

            return new Vector3(rotation.x, angle, rotation.z);
        }
    }
}