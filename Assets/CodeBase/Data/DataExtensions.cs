using UnityEngine;

namespace CodeBase.Data
{
    public static class DataExtensions
    {
        public static Vector3Data AsVectorData(this Vector3 vector) =>
            new Vector3Data(vector.x, vector.y, vector.z);

        public static Vector3 AsUnityVector(this Vector3Data vector3Data) =>
            new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);

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

        public static string ToJson(this object obj) =>
            JsonUtility.ToJson(obj);

        public static T ToDeserialized<T>(this string json) =>
            JsonUtility.FromJson<T>(json);
    }
}