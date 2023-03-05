using CodeBase.Tools.Constants;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class SphereCaster : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private LayerMask _mask;

        private float _colliderRadius;
        private float _colliderHeight;

        private void Awake()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            _colliderHeight = capsuleCollider.height;
            _colliderRadius = capsuleCollider.radius;
        }

        public bool CastSphere(Vector3 direction, float distance, float radiusReduction = 0.1f)
        {
            bool isHit = Physics.SphereCast(_rigidbody.position, _colliderRadius - radiusReduction, direction,
                out _, distance + _colliderHeight * Arithmetic.ToHalf - _colliderRadius, _mask);
            return isHit;
        }
    }
}