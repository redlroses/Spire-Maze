﻿using System.Diagnostics.CodeAnalysis;
using CodeBase.Tools.Constants;
using UnityEngine;

namespace CodeBase.Logic.Movement
{
    [RequireComponent(typeof(CapsuleCollider))]
    [SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
    public class SphereCaster : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private LayerMask _mask;

        private float _colliderRadius;
        private float _colliderHeight;

        private void Awake()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
            _collider ??= GetComponent<CapsuleCollider>();
            _colliderHeight = _collider.height;
            _colliderRadius = _collider.radius;
        }

        public bool CastSphere(Vector3 direction, float distance, float radiusReduction = 0.1f)
        {
            bool isHit = Physics.SphereCast(
                _rigidbody.position + _collider.center,
                _colliderRadius - radiusReduction,
                direction,
                out _,
                distance + (_colliderHeight * Arithmetic.ToHalf),
                _mask);

            return isHit;
        }

        public bool CastSphere(Vector3 direction, float distance, out RaycastHit hitInfo, float radiusReduction = 0.1f)
        {
            bool isHit = Physics.SphereCast(
                _rigidbody.position + _collider.center,
                _colliderRadius - radiusReduction,
                direction,
                out hitInfo,
                distance + (_colliderHeight * Arithmetic.ToHalf),
                _mask);

            return isHit;
        }
    }
}