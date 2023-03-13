using CodeBase.Logic.Movement;
using UnityEngine;

namespace CodeBase.Tools
{
    public static class RayDirection
    {
        public static Vector3 Calculate(Vector3 anchorPoint, Vector3 currentPoint, MoveDirection direction)
        {
            Vector3 directionForAnchor = new Vector3(anchorPoint.x, currentPoint.y, anchorPoint.z) - currentPoint;
            return Vector3.Cross(directionForAnchor, Vector3.down * (int)direction).normalized;
        }
    }
}