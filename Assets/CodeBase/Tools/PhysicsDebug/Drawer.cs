using UnityEngine;

namespace CodeBase.Tools.PhysicsDebug
{
    public static class Drawer
    {
        public static void DrawRay(Vector3 from, Vector3 to, Color color, float lifetime = 1f)
        {
            Debug.DrawRay(from, to, color, lifetime);
        }

        public static void DrawRay(Vector3 from, Vector3 direction, float length, Color color, float lifetime = 1f)
        {
            Debug.DrawRay(from, direction.normalized * length, color, lifetime);
        }
    }
}