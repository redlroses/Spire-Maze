using CodeBase.Tools.Extension;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    public class VirtualMover : MonoCache
    {
        [SerializeField] private float _speed = 10f;

        public void Move(Vector2 direction)
        {
            Vector3 worldDirection = direction.ToWorldDirection(transform.position, Spire.DistanceToCenter).ChangeY(direction.y);
            transform.Translate(worldDirection.normalized * _speed * Time.deltaTime);
        }
    }
}