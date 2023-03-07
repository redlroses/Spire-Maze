using UnityEngine;

namespace CodeBase.Logic.Portal
{
    [RequireComponent(typeof(Rigidbody))]
    public class Teleportable : MonoBehaviour, ITeleportable
    {
        [SerializeField] private Rigidbody _rigidbody;

        public Vector3 Forward => _rigidbody.transform.forward;

        private void Awake()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        public void Teleportation(Vector3 position, Vector3 rotation)
        {
            _rigidbody.position = position;
            _rigidbody.rotation = Quaternion.LookRotation(rotation);
        }
    }
}
