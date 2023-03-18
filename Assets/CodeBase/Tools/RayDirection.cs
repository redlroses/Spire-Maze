using CodeBase.Logic.Movement;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Tools
{
    public class RayDirection : MonoCache
    {
        public Vector3 Calculate(Vector3 anchorPoint, Vector3 currentPoint, MoveDirection direction)
        {
            Vector3 directionForAnchor = new Vector3(anchorPoint.x, currentPoint.y, anchorPoint.z) - currentPoint;
            return Vector3.Cross(directionForAnchor, Vector3.down * (int) direction).normalized;
        }
    }
}