using UnityEngine;
using NTC.Global.Cache;
using CodeBase.Logic;

namespace CodeBase.Infrastructure
{
    public class RoundTransform : MonoCache
    {
        [Range(MinAngle, MaxAngle)]
        [SerializeField] private float _angle;
        [SerializeField] private float _positionY;

        private const float MinAngle = 0f;
        private const float MaxAngle = 2 * Mathf.PI;

        private void OnValidate()
        {
            Move();
        }

        private void Move()
        {
            float positionX = Spire.Position.x + Mathf.Cos(_angle) * Spire.DistanceToCenter;
            float positionZ = Spire.Position.z + Mathf.Sin(_angle) * Spire.DistanceToCenter;
            Vector3 direction = new Vector3(positionX, _positionY, positionZ);

            transform.position = direction;
        }
    }
}