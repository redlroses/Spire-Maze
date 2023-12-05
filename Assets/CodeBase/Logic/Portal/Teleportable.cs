using System;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Logic.Portal
{
    [RequireComponent(typeof(Rigidbody))]
    public class Teleportable : MonoBehaviour, ITeleportable
    {
        private const float OffsetY = 0.85f;
        
        [SerializeField] private Rigidbody _rigidbody;

        public event Action Teleported = () => { };

        public Vector3 Forward => _rigidbody.transform.forward;

        private void Awake()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        public void Teleportation(Vector3 position, Vector3 rotation)
        {
            _rigidbody.Sleep();
            _rigidbody.position = position.ChangeY(position.y-OffsetY);
            _rigidbody.rotation = Quaternion.LookRotation(rotation);
            _rigidbody.WakeUp();
            Teleported.Invoke();
        }
    }
}
